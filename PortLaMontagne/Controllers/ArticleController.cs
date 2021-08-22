#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortLaMontagne.Data;
using PortLaMontagne.Models;
using PortLaMontagne.Models.Views;
using PortLaMontagne.Repository;
using PortLaMontagne.Services;

namespace PortLaMontagne.Controllers
{
    [Route("articles")]
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly ArticleRepository _articleRepository;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly UploadFile _uploadFile;
        private readonly IConfiguration _configuration;

        public ArticleController(ILogger<ArticleController> logger, ApplicationDbContext dbContext,
            ArticleRepository articleRepository, UserManager<ApplicationUser> manager, UploadFile uploadFile, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _articleRepository = articleRepository;
            _manager = manager;
            _uploadFile = uploadFile;
            _configuration = configuration;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _articleRepository.FindActiveAsync(true));
        }

        [Route("{slug}")]
        public async Task<IActionResult> Details(string? slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var article = await _articleRepository.FindBySlugWithEditorAsync(slug);


            if (article == null)
            {
                return NotFound();
            }

            return View(new ArticleDetailsView
            {
                Article = article,
                RecentArticles = await _dbContext.Articles
                    .Where(a => a.IsPublished && a.Slug != slug).Take(4)
                    .ToListAsync()
            });
        }

        [Authorize]
        [Route("create")]
        public IActionResult Create()
        {
            return View(new Article());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Title, Content, IsPublished, Category, FormFile")]
            Article article)
        {
            if (!ModelState.IsValid) return View();
            
            var art = article;
            art.Image = await _uploadFile.UploadFormFile(article.FormFile, _configuration.GetSection("defaultPath").GetSection("ImageArticle").Value);
            art.CreatedAt = DateTime.Now;
            article.Editor = await _manager.GetUserAsync(HttpContext.User);
            
            _dbContext.Add(article);
            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}