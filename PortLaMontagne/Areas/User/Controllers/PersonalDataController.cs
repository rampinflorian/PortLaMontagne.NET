using System.Threading.Tasks;
using Core.Flash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortLaMontagne.Areas.User.Form.PersonalData;
using PortLaMontagne.Data;
using PortLaMontagne.Models;
using PortLaMontagne.Services;

namespace PortLaMontagne.Areas.User.Controllers
{
    [Area("user")]
    [Authorize(Roles = "Admin, User")]
    [Route("users/personal-data")]
    public class PersonalDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFlasher _flasher;
        private readonly WwwRootService _wwwRootService;
        private readonly IConfiguration _configuration;

        public PersonalDataController(UserManager<ApplicationUser> manager, ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager, IFlasher flasher, WwwRootService wwwRootService,
            IConfiguration configuration)
        {
            _manager = manager;
            _context = context;
            _signInManager = signInManager;
            _flasher = flasher;
            _wwwRootService = wwwRootService;
            _configuration = configuration;
        }

        [Route("", Name = "index.personaldata.index")]
        public async Task<IActionResult> Index()
        {
            var applicationUser = await _manager.GetUserAsync(User);

            return View(new InputForm()
            {
                AdministrativeForm = new AdministrativeForm()
                {
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Description = applicationUser.Description
                },
                UserForm = new UserForm()
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("", Name = "index.personaldata.index.edit")]
        public async Task<IActionResult> Index(InputForm inputForm)
        {
            if (inputForm is null)
            {
                return NotFound();
            }

            var applicationUserToUpdate = await _manager.GetUserAsync(User);

            if (inputForm.AdministrativeForm.FirstName != applicationUserToUpdate.FirstName ||
                inputForm.AdministrativeForm.LastName != applicationUserToUpdate.LastName ||
                inputForm.AdministrativeForm.Description != applicationUserToUpdate.Description)
            {
                applicationUserToUpdate.FirstName = inputForm.AdministrativeForm.FirstName;
                applicationUserToUpdate.LastName = inputForm.AdministrativeForm.LastName;
                applicationUserToUpdate.Description = inputForm.AdministrativeForm.Description;
                _context.Update(applicationUserToUpdate);

            }

            if (inputForm.AdministrativeForm.FormFile is not null)
            {
                applicationUserToUpdate.Image = await _wwwRootService.UploadFormFile(
                    inputForm.AdministrativeForm.FormFile,
                    _configuration.GetSection("defaultPath").GetSection("ImageUser").Value,
                    applicationUserToUpdate.Image);
                _context.Update(applicationUserToUpdate);

            }


            await _context.SaveChangesAsync();


            if (inputForm.UserForm.OldPassword != null)
            {
                var changePasswordResult = await _manager.ChangePasswordAsync(applicationUserToUpdate,
                    inputForm.UserForm.OldPassword, inputForm.UserForm.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(applicationUserToUpdate);
                _flasher.Flash(Types.Info, "Le mot de passe a été modifié.", true);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}