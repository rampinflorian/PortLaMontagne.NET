using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortLaMontagne.Data;
using PortLaMontagne.Models;
using PortLaMontagne.Services;

namespace PortLaMontagne.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("admin/articles")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WwwRootService _wwwRootService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(ApplicationDbContext context, WwwRootService wwwRootService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _wwwRootService = wwwRootService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [Route("", Name = "admin.articles.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Articles = await _context.Articles
                .AsNoTracking()
                .Include(m => m.Comments).OrderByDescending(m => m.CreatedAt).ToListAsync();
            return View();
        }

        [Route("create", Name = "admin.article.create")]
        public IActionResult Create()
        {
            return View(new Article());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create", Name = "admin.article.create")]
        public async Task<IActionResult> Create(
            [Bind("Title, Content, IsPublished, Category, FormFile")]
            Article article)
        {
            if (!ModelState.IsValid) return View();

            article.Image = await _wwwRootService.UploadFormFile(article.FormFile,
                _configuration.GetSection("defaultPath").GetSection("ImageArticle").Value);
            article.CreatedAt = DateTime.Now;
            article.Editor = await _userManager.GetUserAsync(HttpContext.User);

            _context.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("edit/{slug}", Name = "admin.article.edit")]
        public async Task<IActionResult> Edit(string slug)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Slug == slug);

            if (article is null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{slug}", Name = "admin.article.edit.post")]
        public async Task<IActionResult> Edit(string slug, int articleId, [Bind("ArticleId, Title, Content, IsPublished, Category, FormFile, Image")] Article article)
        {
            if (articleId != article.ArticleId)
            {
                return NotFound();
            }

            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            article.Image = await _wwwRootService.UploadFormFile(article.FormFile,
                _configuration.GetSection("defaultPath").GetSection("ImageArticle").Value,
                article.Image);

            _context.Update(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("delete", Name = "admin.article.delete")]
    public async Task<IActionResult> Delete(int articleId)
    {
    var article = await _context.Articles.Include(m => m.Comments)
            .FirstOrDefaultAsync(m => m.ArticleId == articleId);
        if (article is null)
    {
        return NotFound();
    }

    _wwwRootService.DeleteFile(Path.Combine(_configuration.GetSection(
        "defaultPath").GetSection("ImageArticle").Value, article.Image));
    _context.Remove(article);

    await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

}