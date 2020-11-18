using Dapper;
using restlessmedia.Module.Address;
using restlessmedia.Module.Data;
using restlessmedia.Module.Data.Sql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace restlessmedia.Module.Place.Data
{
  public class PlaceSqlDataProvider : SqlDataProviderBase
  {
    public PlaceSqlDataProvider(IDataContext context)
      : base(context) { }

    public TPlace Read<TPlace, TAddress, TMarker>(int placeId)
      where TPlace : PlaceEntity
      where TAddress : AddressEntity, new()
      where TMarker : Marker
    {
      return Query((connection) =>
      {
        return connection.Query<TPlace, TAddress, TMarker, TPlace>("dbo.SPReadPlace", (p, a, m) =>
        {
          a = a ?? new TAddress();
          p.Address = a;
          a.Marker = m;
          return p;
        }, new { placeId }, commandType: CommandType.StoredProcedure, splitOn: "AddressId,Latitude").FirstOrDefault();
      });
    }

    public void Save(PlaceEntity place)
    {
      // TODO: fix up magic number
      place.Address?.Marker?.Round(6);

      using (IDbConnection connection =  DataContext.ConnectionFactory.CreateConnection())
      {
        DynamicParameters parameters = new DynamicParameters(new
        {
          place.PlaceType,
          place.Address?.AddressId,
          place.Address?.KnownAs,
          place.Address?.NameNumber,
          place.Address?.Address01,
          place.Address?.Address02,
          place.Address?.PostCode,
          place.Address?.CountryCode,
          place.Address?.Town,
          place.Address?.City,
          place.Address?.Marker?.Latitude,
          place.Address?.Marker?.Longitude,
        });

        parameters.Add("placeId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        connection.Execute("dbo.SPSavePlace", parameters, commandType: CommandType.StoredProcedure);

        // set the id to the identity
        place.PlaceId = parameters.Get<int>("placeId");
      }
    }

    public PlaceEntity NearestPlace(PlaceType type, double latitude, double longitude)
    {
      return Query<PlaceEntity>("dbo.SPNearestPlace", new { placeType = (int)type, latitude, longitude }).FirstOrDefault();
    }

    public Nearest Nearest(PlaceType type, double latitude, double longitude)
    {
      const string sql = @"
          SELECT TOP 1
            P.KnownAs Title
          , P.Latitude
          , P.Longitude
        FROM dbo.VPlace P
        WHERE P.LicenseId = @licenseId
        ORDER BY dbo.GetDistanceBetween(@latitude, @longitude, P.Latitude, P.Longitude) ASC";

      return Query((connection) =>
      {
        return connection.Query<Nearest, Marker, Nearest>(sql, (n, m) => { n.Marker = m; return n; }, new { placeType = (int)type, latitude, longitude, licenseId = LicenseHelper.GetLicenseId(connection, DataContext.LicenseSettings) }, commandType: CommandType.Text, splitOn: "Latitude")
          .FirstOrDefault();
      });
    }

    public ModelCollection<AddressEntity> ListPartialMarkers()
    {
      return ModelQuery<AddressEntity>("dbo.SPListPartialMarkers");
    }

    public void UpdateMarker(AddressEntity address, Marker marker)
    {
      Execute("dbo.SPUpdateMarker", new { addressId = address.AddressId, latitude = marker.Latitude, longitude = marker.Longitude });
    }

    public ModelCollection<PlaceEntity> List(int page, int maxPerPage, bool getCount)
    {
      // we may not have a full address (only marker) so check this and create a stub address to pin the marker to if there's no data
      return Query((connection) =>
      {
        IEnumerable<PlaceEntity> data = connection.Query<PlaceEntity, AddressEntity, Marker, PlaceEntity>("dbo.SPListPlaces", (p, a, m) =>
        {
          p.Address = a ?? new AddressEntity();
          a.Marker = m;
          return p;
        }, new { page, maxPerPage }, commandType: CommandType.StoredProcedure, splitOn: "AddressId,Latitude");

        if (getCount)
        {
          return new ModelCollection<PlaceEntity>(data, connection.Query<int>("dbo.SPListPlaces_count", commandType: CommandType.StoredProcedure).First());
        }

        return new ModelCollection<PlaceEntity>(data);
      });
    }
  }
}