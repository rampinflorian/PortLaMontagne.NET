using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;

namespace PortLaMontagne.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("", Name = "admin.index")]
        public IActionResult Index()
        {
            ViewBag.UsersCount = _context.ApplicationUsers.AsNoTracking().Count();
            ViewBag.ArticlesCount = _context.Articles.AsNoTracking().Count(m => m.IsPublished);
            ViewBag.CommentsCount = _context.Comments.AsNoTracking().Count();
            
            return View();
        }
    }
}