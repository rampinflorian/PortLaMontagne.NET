using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortLaMontagne.Data;
using PortLaMontagne.Models;
using PortLaMontagne.Services;

namespace PortLaMontagne.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/partner")]
    [Authorize(Roles = "Admin")]
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WwwRootService _wwwRootService;
        private readonly IConfiguration _configuration;

        public PartnerController(ApplicationDbContext context, WwwRootService wwwRootService,
            IConfiguration configuration)
        {
            _context = context;
            _wwwRootService = wwwRootService;
            _configuration = configuration;
        }

        [Route("", Name = "admin.partner.index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Partners = await _context.Partners.ToListAsync();

            return View();
        }

        [Route("create", Name = "admin.partner.create")]
        public IActionResult Create()
        {
            return View(new Partner());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("create", Name = "admin.partner.create")]
        public async Task<IActionResult> Create([Bind("Name, Website, FormFile")] Partner partner)
        {
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            partner.Image = await _wwwRootService.UploadFormFile(partner.FormFile,
                _configuration.GetSection("defaultPath").GetSection("ImagePartner").Value);

            await _context.AddAsync(partner);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("edit", Name = "admin.partner.edit")]
        public async Task<IActionResult> Edit(int partnerId)
        {
            var partner = await _context.Partners.FirstOrDefaultAsync(m => m.PartnerId == partnerId);
            
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit", Name = "admin.partner.edit.post")]
        public async Task<IActionResult> Edit(int partnerId, [Bind("PartnerId, Name, Website, FormFile")] Partner partner)
        {
            if (partnerId != partner.PartnerId)
            {
                return NotFound();
            }
            
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(partner);
            }
            
            partner.Image = await _wwwRootService.UploadFormFile(partner.FormFile,
                _configuration.GetSection("defaultPath").GetSection("ImagePartner").Value,partner.Image);
            
            _context.Update(partner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete", Name = "admin.partner.delete")]
        public async Task<IActionResult> Delete(int partnerId)
        {
            var partner = await _context.Partners.FirstOrDefaultAsync(m => m.PartnerId == partnerId);

            if (partner is null)
            {
                return NotFound();
            }
            
            _wwwRootService.DeleteFile(Path.Combine(_configuration.GetSection("defaultPath").GetSection("ImagePartner").Value, partner.Image));
            
            _context.Remove(partner);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}