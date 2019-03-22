using restlessmedia.Module.Address;

namespace restlessmedia.Module.Place
{
  public interface IPlace
  {
    int? PlaceId { get; set; }

    IAddress Address { get; set; }

    PlaceType PlaceType { get; set; }
  }
}
