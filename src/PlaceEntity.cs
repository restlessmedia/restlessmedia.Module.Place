using restlessmedia.Module.Address;

namespace restlessmedia.Module.Place
{
  public class PlaceEntity : Entity, IPlace
  {
    public PlaceEntity() { }

    public PlaceEntity(PlaceType type)
      : this()
    {
      PlaceType = type;
    }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.Place;
      }
    }

    public override int? EntityId
    {
      get
      {
        return PlaceId;
      }
    }

    public override string Title
    {
      get
      {
        return Address.KnownAs;
      }
    }

    public int? PlaceId { get; set; }

    public IAddress Address
    {
      get
      {
        return _address = _address ?? new AddressEntity();
      }
      set
      {
        _address = value;
      }
    }

    public PlaceType PlaceType { get; set; }

    private IAddress _address = null;
  }
}