using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restlessmedia.Module.Place.Data
{
  [Table("VPlace")]
  internal class VPlace
  {
    public VPlace() { }

    public VPlace(IPlace place)
    {
      PlaceId = place.PlaceId;
      AddressId = place.Address?.AddressId;
      PlaceType = place.PlaceType;
      KnownAs = place.Address?.KnownAs;
      NameNumber = place.Address?.NameNumber;
      Address01 = place.Address?.Address01;
      Address02 = place.Address?.Address02;
      Town = place.Address?.Town;
      City = place.Address?.City;
      CountryCode = place.Address?.CountryCode;
      PostCode = place.Address?.PostCode;
      Latitude = place.Address?.Marker?.Latitude;
      Longitude = place.Address?.Marker?.Longitude;
    }

    [Key]
    public int? PlaceId { get; set; }

    public int? AddressId { get; set; }

    public PlaceType PlaceType { get; set; }

    public string KnownAs { get; set; }

    public string NameNumber { get; set; }

    public string Address01 { get; set; }

    public string Address02 { get; set; }

    public string Town { get; set; }

    public string City { get; set; }

    public string CountryCode { get; set; }

    public string PostCode { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? LicenseId { get; set; }

    public int? EntityGuid { get; set; }
  }
}