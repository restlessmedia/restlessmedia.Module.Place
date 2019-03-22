using restlessmedia.Module.Data;

namespace restlessmedia.Module.Place.Data
{
  public class DatabaseContext : restlessmedia.Module.Data.EF.DatabaseContext<VPlace>
  {
    public DatabaseContext(IDataContext dataContext)
      : base(dataContext) { }
  }
}