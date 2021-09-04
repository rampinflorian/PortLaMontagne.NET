using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;
using PortLaMontagne.Models;

namespace PortLaMontagne.Areas.User.Controllers
{
    [Area("user")]
    [Route("users/market")]
    [Authorize(Roles = "Admin, User")]
    public class MarketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarketController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("", Name = "user.market.index")]
        public async Task<IActionResult> Index()
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            ViewBag.MarketProducts = await _context.MarketProducts.Where(m => m.Vendor == applicationUser).ToListAsync();

            return View();
        }

        [Route("delete", Name = "user.market.delete")]
        public IActionResult Delete(int marketProductId)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}