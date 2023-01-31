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
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, 
            IMailService localMailService,
            CitiesDataStore citiesDataStore) // Constructor injection
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // If constructor injection is not feasible, we can request containers in this way
            // HttpContext.RequestServices.GetService() but constructor injection is always preferred
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                // throw new Exception("Exception sample");
                var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
                if (city != null) return Ok(city.PointsOfInterest);

                _logger.LogInformation("City with ID: {CityId} wasn't found when accessing points of interest", cityId);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogCritical("City with ID: {CityId} wasn't found when accessing points of interest." +
                                    "Error Message: {Message}", cityId, e.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound();

            // TODO: Needs to be improved
            var maxId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(point => point.Id);

            var finalPointOfInterest = new PointOfInterestDto
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest);
        }

        // PUT: Full updates
        [HttpPut("{pointOfInterestId:int}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdate)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            // Full update principle || User must include all fields when sending the PUT request
            // If any field is missing, it will be set to default value or null
            pointOfInterest.Name = pointOfInterestForUpdate.Name;
            pointOfInterest.Description = pointOfInterestForUpdate.Description;

            return NoContent();
        }

        // PATCH: Partial update
        [HttpPatch("{pointOfInterestId:int}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState); // Check if the Dto is valid after aplying patch

            pointOfInterest.Name = pointOfInterestToPatch.Name;
            pointOfInterest.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId:int}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterest);
            _localMailService.Send("Point of interest deleted", pointOfInterest.ToString());

            return NoContent();
        }
    }
}