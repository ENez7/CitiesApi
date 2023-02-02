using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Base link for this controller
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    // [HttpGet("{id:int}")] // Path parameter
    // public ActionResult<CityDto> GetCity(int id)
    // {
    //     var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);
    //     if (city == null) return NotFound();
    //     return Ok(city);
    // }
}