using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;

namespace PortLaMontagne.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/comments")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("", Name = "admin.comment.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Comments = await _context.Comments
                .Include(m => m.Article)
                .Include(m => m.ApplicationUser)
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete", Name = "admin.comment.delete")]
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