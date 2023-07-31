using CityInfo.Models.Cities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers.Cities
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId) 
        { 
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();
            return Ok(city.PointsOfInterest);
        }
    }
}
