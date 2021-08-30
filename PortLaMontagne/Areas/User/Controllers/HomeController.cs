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
    [Authorize(Roles = "Admin, User")]
    [Route("users")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _manager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        [Route("", Name = "user.home.index")]
        public async Task<IActionResult> Index()
        {
            var applicationUser = await _manager.GetUserAsync(User);

            ViewBag.Comments = await _context.Comments
                .Include(m => m.ApplicationUser)
                .Include(m => m.Article)
                .Where(m => m.ApplicationUser == applicationUser).ToListAsync();
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete", Name = "user.home.delete")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(m => m.CommentId == commentId);

            if (comment is null)
            {
                return NotFound();
            }

            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}