using Autofac;
using restlessmedia.Module.Place.Data;

namespace restlessmedia.Module.Place
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterType<GoogleGeoProvider>().As<IGeoProvider>().SingleInstance(); 
      containerBuilder.RegisterType<PlaceService>().As<IPlaceService>().SingleInstance();
      containerBuilder.RegisterType<PlaceDataProvider>().As<IPlaceDataProvider>().SingleInstance();
    }
  }
}