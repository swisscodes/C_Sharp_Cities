using CityInfo.Models.Cities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers.Cities
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController(CitiesDataStore citiesDataStore) : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));

        //public CitiesController(CitiesDataStore citiesDataStore)
        //{
        //    _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        //}

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var resultToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
            if (resultToReturn == null)
            {
                return NotFound();
            }
            Console.WriteLine("hello world 1");
            return Ok(resultToReturn);
        }
    }
}
