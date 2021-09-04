using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;

namespace PortLaMontagne.Controllers
{
    [Route("market")]
    public class MarketController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MarketController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("", Name = "market.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.MarketProducts = await _context.MarketProducts
                .Include(m => m.Vendor)
                .OrderByDescending(m => m.CreatedAt).ToListAsync();
            
            return View();
        }

        [Route("{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var marketProduct = await _context.MarketProducts.AsNoTracking()
                .Include(m => m.Vendor)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (marketProduct.Slug != slug)
            {
                return NotFound();
            }

            ViewBag.MarketProduct = marketProduct;

            return View();

        }
    }
}