using restlessmedia.Module.Data.EF;
using System.Linq;

namespace restlessmedia.Module.Place.Data
{
  public class PlaceRepository : LicensedEntityRepository<VPlace>
  {
    public PlaceRepository(DatabaseContext context)
      : base(context)
    {
      _databaseContext = context;
    }

    public VPlace Save(IPlace place)
    {
      VPlace dataModel = new VPlace(place);

      if (_databaseContext.Data.Any(x => x.PlaceId == place.PlaceId))
      {
        Update(dataModel,
          x => x.PlaceId,
          x => x.AddressId,
          x => x.PlaceType
      );
      }
      else
      {
        Add(dataModel);
      }

      return dataModel;
    }

    private readonly DatabaseContext _databaseContext;
  }
}