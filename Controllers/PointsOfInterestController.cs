using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId:int}/[controller]")] // This is a child of another resource (City)
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService localMailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper) // Constructor injection
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // If constructor injection is not feasible, we can request containers in this way
            // HttpContext.RequestServices.GetService() but constructor injection is always preferred
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("City with ID: {CityId} was not found", cityId);
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("City with ID: {CityId} was not found", cityId);
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("City with ID: {CityId} was not found", cityId);
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync(); // Any error at this point implies status code 500

            var createdResource = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = createdResource.Id
                },
                createdResource);
        }

        // PUT: Full updates
        [HttpPut("{pointOfInterestId:int}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdate)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("City with ID: {CityId} was not found", cityId);
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                _logger.LogInformation("Point of interest with ID: {PointOfInterestId} was not found",
                    pointOfInterestId);
                return NotFound();
            }

            // Full update principle || User must include all fields when sending the PUT request
            // If any field is missing, it will be set to default value or null
            _mapper.Map(pointOfInterestForUpdate, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: Partial update
        [HttpPatch("{pointOfInterestId:int}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("City with ID: {CityId} was not found", cityId);
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                _logger.LogInformation("Point of interest with ID: {PointOfInterestId} was not found",
                    pointOfInterestId);
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState); // Check if the Dto is valid after aplying patch

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            
            return NoContent();
        }

        // [HttpDelete("{pointOfInterestId:int}")]
        // public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        // {
        //     var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
        //     if (city == null) return NotFound();
        //
        //     var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
        //     if (pointOfInterest == null) return NotFound();
        //
        //     city.PointsOfInterest.Remove(pointOfInterest);
        //     _localMailService.Send("Point of interest deleted", pointOfInterest.ToString());
        //
        //     return NoContent();
        // }
    }
}