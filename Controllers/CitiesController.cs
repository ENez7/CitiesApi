using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Base link for this controller
public class CitiesController : ControllerBase
{
    private readonly CitiesDataStore _citiesDataStore;

    public CitiesController(CitiesDataStore citiesDataStore)
    {
        _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
        return Ok(_citiesDataStore.Cities);  // An empty return is ok as well
    }

    [HttpGet("{id:int}")] // Path parameter
    public ActionResult<CityDto> GetCity(int id)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);
        if (city == null) return NotFound();
        return Ok(city);
    }
}