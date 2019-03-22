using restlessmedia.Module.Address;

namespace restlessmedia.Module.Place
{
  public interface IPlaceService : IService
  {
    TPlace Read<TPlace, TAddress, TMarker>(int placeId, bool employCache = false)
      where TPlace : PlaceEntity
      where TAddress : AddressEntity, new()
      where TMarker : Marker;

    void Save(PlaceEntity place);

    void Save(ModelCollection<PlaceEntity> places);

    Nearest Nearest(PlaceType type, double latitude, double longitude);

    Nearest Nearest(PlaceType type, Marker marker);

    PlaceEntity NearestPlace(PlaceType type, Marker marker);

    PlaceEntity NearestPlace(PlaceType type, double latitude, double longitude);

    Marker FindMarker(string address);

    AddressEntity Find(double latitude, double longitude, PlaceType type = PlaceType.Station);

    /// <summary>
    /// This will attempt to discver latitude and logitude markers for addresses that don't yet have them
    /// </summary>
    void DiscoverMarkers();

    ModelCollection<PlaceEntity> List(int page, int maxPerPage, bool getCount);
  }
}