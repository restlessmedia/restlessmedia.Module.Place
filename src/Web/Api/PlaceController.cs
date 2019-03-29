using restlessmedia.Module.Address;
using restlessmedia.Module.Place;
using restlessmedia.Module.Web.Api.Attributes;
using System;
using System.Web.Http;

namespace restlessmedia.Module.Place.Web.Api
{
  public class PlaceController : ApiController
  {
    public PlaceController(IPlaceService placeService)
    {
      _placeService = placeService ?? throw new ArgumentNullException(nameof(placeService));
    }

    [HttpGet]
    [ApplicationCache]
    [Route("api/place/nearest/{type}/{latitude}/{longitude}")]
    public Nearest Nearest(PlaceType type, double latitude, double longitude)
    {
      return _placeService.Nearest(type, latitude, longitude);
    }

    [HttpGet]
    [ApplicationCache]
    [Route("api/place/address/{latitude}/{longitude}")]
    public AddressEntity Address(double latitude, double longitude)
    {
      return _placeService.Find(latitude, longitude);
    }

    private readonly IPlaceService _placeService;
  }
}
