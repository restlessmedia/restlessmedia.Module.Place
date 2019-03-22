using restlessmedia.Module.Address;

namespace restlessmedia.Module.Place
{
  public interface IGeoProvider : IProvider
  {
    Marker FindMarker(string address);

    AddressEntity Find(double latitude, double longitude, PlaceType type = PlaceType.Station);
  }
}