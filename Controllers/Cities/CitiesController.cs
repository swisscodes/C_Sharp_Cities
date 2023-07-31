using CityInfo.Models.Cities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers.Cities
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var resultToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (resultToReturn == null)
            {
                return NotFound();
            }
            Console.WriteLine("hello world 1");
            return Ok(resultToReturn);
        }
    }
}
