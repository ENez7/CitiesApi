using System.Text.Json;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
// [Authorize]
[Route("api/[controller]")] // Base link for this controller
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private const int Maxcitiespagesize = 20;
    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery]string? cityName, string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > Maxcitiespagesize) pageSize = Maxcitiespagesize;
        
        // TODO: Add cities with or without points of interest
        var (cityEntities, paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(cityName, searchQuery, pageNumber, pageSize);
        // Pagination metadata ever goes in headers of the response
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id:int}")] // Path parameter
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
        if (city == null) return NotFound();
        
        if (includePointsOfInterest) return Ok(_mapper.Map<CityDto>(city));
        
        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}