using restlessmedia.Module.Address;
using restlessmedia.Module.Data;

namespace restlessmedia.Module.Place.Data
{
  public interface IPlaceDataProvider : IDataProvider
  {
    TPlace Read<TPlace, TAddress, TMarker>(int placeId)
      where TPlace : PlaceEntity
      where TAddress : AddressEntity, new()
      where TMarker : Marker;

    void Save(PlaceEntity place);

    PlaceEntity NearestPlace(PlaceType type, double latitude, double longitude);

    Nearest Nearest(PlaceType type, double latitude, double longitude);

    ModelCollection<AddressEntity> ListPartialMarkers();

    void UpdateMarker(AddressEntity address, Marker marker);

    ModelCollection<PlaceEntity> List(int page, int maxPerPage, bool getCount);
  }
}