using restlessmedia.Module.Address;
using restlessmedia.Module.Place.Data;
using System;

namespace restlessmedia.Module.Place
{
  public class PlaceService : IPlaceService
  {
    public PlaceService(IGeoProvider geoProvider, IPlaceDataProvider placeDataProvider)
    {
      _geoProvider = geoProvider ?? throw new ArgumentNullException(nameof(geoProvider));
      _placeDataProvider = placeDataProvider ?? throw new ArgumentNullException(nameof(placeDataProvider));
    }

    public TPlace Read<TPlace, TAddress, TMarker>(int placeId, bool employCache = false)
      where TPlace : PlaceEntity
      where TAddress : AddressEntity, new()
      where TMarker : Marker
    {
      return _placeDataProvider.Read<TPlace, TAddress, TMarker>(placeId);
    }

    public void Save(PlaceEntity place)
    {
      Save(ModelCollection<PlaceEntity>.One(place));
    }

    public void Save(ModelCollection<PlaceEntity> places)
    {
      _placeDataProvider.Save(places);
    }

    public Nearest Nearest(PlaceType type, double latitude, double longitude)
    {
      return _placeDataProvider.Nearest(type, latitude, longitude);
    }

    public Nearest Nearest(PlaceType type, Marker marker)
    {
      return marker.Latitude.HasValue && marker.Longitude.HasValue ? Nearest(type, marker.Latitude.Value, marker.Longitude.Value) : null;
    }

    public PlaceEntity NearestPlace(PlaceType type, Marker marker)
    {
      return marker.Latitude.HasValue && marker.Longitude.HasValue ? NearestPlace(type, marker.Latitude.Value, marker.Longitude.Value) : null;
    }

    public PlaceEntity NearestPlace(PlaceType type, double latitude, double longitude)
    {
      return _placeDataProvider.NearestPlace(type, latitude, longitude);
    }

    public Marker FindMarker(string address)
    {
      return _geoProvider.FindMarker(address);
    }

    public AddressEntity Find(double latitude, double longitude, PlaceType type = PlaceType.Station)
    {
      return _geoProvider.Find(latitude, longitude, type);
    }

    public void DiscoverMarkers()
    {
      ModelCollection<AddressEntity> list = _placeDataProvider.ListPartialMarkers();

      foreach (AddressEntity address in list)
      {
        string fullAddress = address.ToString(",");
        Marker marker = FindMarker(fullAddress);

        // update
        if (marker != null)
        {
          _placeDataProvider.UpdateMarker(address, marker);
        }
      }
    }

    public ModelCollection<PlaceEntity> List(int page, int maxPerPage, bool getCount)
    {
      ModelCollection<PlaceEntity> list = _placeDataProvider.List(page, maxPerPage, getCount);
      list.Paging.Page = page;
      list.Paging.MaxPerPage = maxPerPage;
      return list;
    }

    private readonly IGeoProvider _geoProvider;

    private readonly IPlaceDataProvider _placeDataProvider;
  }
}