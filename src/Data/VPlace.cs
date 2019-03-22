using restlessmedia.Module.Address;
using restlessmedia.Module.Address.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restlessmedia.Module.Place
{
  [Table("VPlace")]
  public class VPlace : LicensedEntity, IPlace
  {
    public VPlace() { }

    public VPlace(IPlace place)
    {
      PlaceId = place.PlaceId;
      PlaceType = place.PlaceType;
      Address = place.Address;
    }

    [Key]
    public int? PlaceId { get; set; }

    public override int? EntityId
    {
      get
      {
        return PlaceId;
      }
    }

    public override EntityType EntityType
    {
      get
      {
        return EntityType.Place;
      }
    }

    public int? AddressId { get; set; }

    public PlaceType PlaceType { get; set; }

    public IAddress Address
    {
      get
      {
        return VAddress;
      }
      set
      {
        VAddress = new VAddress(value);
      }
    }

    [ForeignKey("AddressId")]
    public VAddress VAddress { get; set; }
  }
}