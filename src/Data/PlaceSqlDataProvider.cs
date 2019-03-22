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
      : base(context)
    { }

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
        }, new { placeId = placeId }, commandType: CommandType.StoredProcedure, splitOn: "AddressId,Latitude").FirstOrDefault();
      });
    }

    public void Save(ModelCollection<PlaceEntity> places)
    {
      using (DatabaseContext context = CreateDatabaseContext())
      {
        foreach (PlaceEntity place in places)
        {
          // TODO: fix up magic number
          place.Address.Marker.Round(6);

          PlaceRepository placeRepository = new PlaceRepository(context);
          VPlace vPlace = placeRepository.Save(place);
          context.SaveChanges();
          place.PlaceId = vPlace.PlaceId;
        }
      }
    }

    public PlaceEntity NearestPlace(PlaceType type, double latitude, double longitude)
    {
      return Query<PlaceEntity>("dbo.SPNearestPlace", new { placeType = (int)type, latitude = latitude, longitude = longitude }).FirstOrDefault();
    }

    public Nearest Nearest(PlaceType type, double latitude, double longitude)
    {
      return Query((connection) =>
      {
        return connection.Query<Nearest, Marker, Nearest>("dbo.SPNearestPlaceTitle", (n, m) => { n.Marker = m; return n; }, new { placeType = (int)type, latitude = latitude, longitude = longitude }, commandType: CommandType.StoredProcedure, splitOn: "Latitude").FirstOrDefault();
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
       }, new { page = page, maxPerPage = maxPerPage }, commandType: CommandType.StoredProcedure, splitOn: "AddressId,Latitude");

        if (getCount)
        {
          return new ModelCollection<PlaceEntity>(data, connection.Query<int>("dbo.SPListPlaces_count", commandType: CommandType.StoredProcedure).First());
        }

        return new ModelCollection<PlaceEntity>(data);
      });
    }

    private DatabaseContext CreateDatabaseContext()
    {
      return new DatabaseContext(DataContext);
    }
  }
}