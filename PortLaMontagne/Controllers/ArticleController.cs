using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;
using PortLaMontagne.Models;

namespace PortLaMontagne.Controllers
{
    [Route("articles")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("", Name = "article.index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.Include(m => m.Editor).Where(m => m.IsPublished)
                .OrderByDescending(m => m.CreatedAt).ToListAsync());
        }

        [Route("{slug}", Name = "article.details")]
        public async Task<IActionResult> Details(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.Include(m => m.Editor).Where(m => m.Slug == slug)
                .FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound();
            }

            ViewBag.Article = article;
            ViewBag.Comments = await _context.Comments.Where(m => m.Article == article).ToListAsync();
            ViewBag.RecentArticles = await _context.Articles
                .Where(a => a.IsPublished && a.Slug != slug).Take(4).ToListAsync();

            return View(new Comment()
            {
                Article = article
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{slug}/comment", Name = "article.comment")]
        public async Task<IActionResult> Comment(Comment comment, string slug)
        {
            if (comment == null)
            {
                return NoContent();
            }

            var article = await _context.Articles.Where(m => m.Slug == slug)
                .FirstOrDefaultAsync();
            
            if (article == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var isAlreadyCommented =
                _context.Comments.Any(m => m.ApplicationUser == currentUser && m.Article == article);

            if (isAlreadyCommented)
            {
                return NotFound();
            }
            
            comment.Article = article;
            comment.ApplicationUser = currentUser;
            comment.CreatedAt = DateTime.Now;

            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Details), new { slug = comment.Article.Slug });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{slug}/comment/delete", Name = "article.comment.delete")]
        public async Task<IActionResult> CommentDelete(string slug, int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(m => m.CommentId == commentId);

            if (comment == null)
            {
                return NotFound();
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Details), new { slug });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{slug}/newsletter/update", Name = "article.newsletter.updatestatus")]
        public async Task<IActionResult> UpdateNewsletterStatus(string slug)
        {
            var applicationUser = await _userManager.GetUserAsync(HttpContext.User);

            if (applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.IsNewsletter = !applicationUser.IsNewsletter;

            _context.Update(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { slug });
        }
    }
}