using restlessmedia.Module.Data;

namespace restlessmedia.Module.Place.Data
{
  public class PlaceDataProvider : PlaceSqlDataProvider, IPlaceDataProvider
  {
    public PlaceDataProvider(IDataContext context)
      : base(context) { }
  }
}