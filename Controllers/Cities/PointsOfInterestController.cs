using CityInfo.Models.Cities;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers.Cities
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, LocalMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {

                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                
                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id{cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }
        //point of interest id
        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int poiId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
                int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (ModelState.IsValid)
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    return NotFound(); // adds also 404
                }

                // Demo porposes only i know this is not wise
                var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c =>
                c.PointsOfInterest).Max(p => p.Id);

                var finalPointOfInterests = new PointOfInterestDto()
                {
                    Id = ++maxPointOfInterestId,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description,

                };

                city.PointsOfInterest.Add(finalPointOfInterests);
                var a = new { };
                return CreatedAtRoute("GetPointOfInterest",
                    new
                    {
                        cityId,
                        poiId = finalPointOfInterests.Id,
                    }, finalPointOfInterests);
            }
            return BadRequest();
        }

        [HttpPut("{poiId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int poiId,
            PointOfInterestForUpdatingDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();

        }

        [HttpPatch("{poiId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int poiId, JsonPatchDocument<PointOfInterestForUpdatingDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }


            PointOfInterestForUpdatingDto pointOfInterestToPatch = new()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description,
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (ModelState.IsValid && TryValidateModel(pointOfInterestToPatch))
            {
                pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
                pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{poiId}")]
        public ActionResult DeletePointOfInterest(int cityId, int poiId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDto? pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            return NoContent();
        }
        
    }
}
