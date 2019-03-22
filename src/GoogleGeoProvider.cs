using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using restlessmedia.Module.Address;
using System.Linq;

namespace restlessmedia.Module.Place
{
  internal class GoogleGeoProvider : IGeoProvider
  {
    public GoogleGeoProvider()
      : base() { }

    public Marker FindMarker(string address)
    {
      GeocodingResponse geocode = GoogleMaps.Geocode.Query(new GeocodingRequest()
      {
        Address = address
      });

      Result result = geocode.Results.FirstOrDefault();

      return result != null ? new Marker(result.Geometry.Location.Latitude, result.Geometry.Location.Longitude) : null;
    }

    public AddressEntity Find(double latitude, double longitude, PlaceType type = PlaceType.Station)
    {
      GeocodingResponse geocode = GoogleMaps.Geocode.Query(new GeocodingRequest()
      {
        Location = new Location(latitude, longitude)
      });

      Result result;

      switch (type)
      {
        case PlaceType.Station:
          result = geocode.Results.Where(x => x.Types.Contains("subway_station") || x.Types.Contains("train_station")).FirstOrDefault();
          break;
        default:
          // just pick the first
          result = geocode.Results.FirstOrDefault();
          break;
      }

      if (result != null)
      {
        AddressComponent postCode;
        AddressComponent city;
        AddressComponent establishment;

        return new AddressEntity()
        {
          PostCode = (postCode = result.AddressComponents.FirstOrDefault(x => x.Types.Contains("postal_code"))) != null ? postCode.LongName : null,
          City = (city = result.AddressComponents.FirstOrDefault(x => x.Types.Contains("locality"))) != null ? city.LongName : null,
          KnownAs = (establishment = result.AddressComponents.FirstOrDefault(x => x.Types.Contains("establishment"))) != null ? establishment.LongName : null
        };
      }

      return null;
    }
  }
}
