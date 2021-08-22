using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortLaMontagne.Data;

namespace PortLaMontagne.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("", Name = "home.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Articles = await  _context.Articles.Where(m => m.IsPublished == true).ToListAsync();
            ViewBag.Partners = await _context.Partners.ToListAsync();
            ViewBag.CommentsCount = await _context.Comments.CountAsync();

            return View();
        }
    }
}