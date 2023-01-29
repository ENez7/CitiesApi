using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Base link for this controller
public class CitiesController : ControllerBase
{
    [HttpGet]
    public JsonResult GetCities()
    {
        return new JsonResult(CitiesDataStore.Current.Cities);
    }

    [HttpGet("{id:int}")] // Path parameter
    public JsonResult GetCity(int id)
    {
        return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id));
    }
}