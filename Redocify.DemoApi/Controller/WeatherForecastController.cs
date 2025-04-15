using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Redocify.DemoApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get() => ["Sunny", "Cloudy"];
    }
}
