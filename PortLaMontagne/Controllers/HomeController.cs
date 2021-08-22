using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortLaMontagne.Data;
using PortLaMontagne.Models;
using PortLaMontagne.Models.Views;
using PortLaMontagne.Repository;

namespace PortLaMontagne.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly ArticleRepository _articleRepository;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, ArticleRepository articleRepository)
        {
            _logger = logger;
            _dbContext = dbContext;
            _articleRepository = articleRepository;
        }

        [Route("", Name = "home.index")]
        public async Task<IActionResult> Index()
        {
            return View(new HomeView
            {
                Partner = await _dbContext.Partners.ToListAsync(),
                Articles = await _articleRepository.FindActiveAsync(),
                CountArticles = await _articleRepository.FindActiveByQuantityAsync(4)
            });
        }
    }
}