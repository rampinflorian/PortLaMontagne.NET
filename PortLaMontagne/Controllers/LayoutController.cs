using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortLaMontagne.Services.API;

namespace PortLaMontagne.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public LayoutController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> WeatherMap()
        {
            var owm = new OpenWeatherMapAPI();
            var data = await owm.CallApi();
            return Ok(data);
        }

    }
}