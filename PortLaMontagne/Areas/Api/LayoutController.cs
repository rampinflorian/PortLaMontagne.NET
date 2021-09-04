using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;
using PortLaMontagne.Services.API;

namespace PortLaMontagne.Areas.Api
{
    [Area("Api")]
    [Route("api")]
    public class LayoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LayoutController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("getweathermap", Name = "api.layout.getweathermap")]
        public async Task<IActionResult> GetWeatherMap()
        {
            var owm = new OpenWeatherMapApi();
            var data = await owm.CallApi();
            return Ok(data);
        }

        [Route("getarticlesalertcount", Name = "api.layout.getarticlescount")]
        public async Task<IActionResult> GetArticlesAlertCount()
        {
            return Ok(await _context.Articles.Where(m => m.IsAlert && m.IsPublished).CountAsync());
        }
    }
}