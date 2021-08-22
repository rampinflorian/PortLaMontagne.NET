using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.ReCaptcha;
using Core.Flash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using PortLaMontagne.Data;
using PortLaMontagne.Forms;
using PortLaMontagne.Models;
using PortLaMontagne.Services;

namespace PortLaMontagne.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IFlasher _flasher;
        private readonly MailerService _mailerService;

        public ContactController(ILogger<ContactController> logger, ApplicationDbContext dbContext, IFlasher flasher,
            MailerService mailerService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _flasher = flasher;
            _mailerService = mailerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public async Task<IActionResult> Mailer([Bind("Username, Email, Subject, Content")] ContactForm contactForm)
        {
            if (!ModelState.IsValid)
            {
                var resCaptcha = ModelState.First(k => k.Key == "Recaptcha").Value;
                if (resCaptcha.ValidationState == ModelValidationState.Invalid)
                {
                    _flasher.Flash(Types.Danger, "BIP BOOP BUP ! Tu es un robot ? Il faut valider le Captcha !", true);
                    return RedirectToAction("Index");
                }
            }

            try
            {
                await _mailerService.SendContactAsync(contactForm);
                _flasher.Flash(Types.Success, "Ton message a bien été envoyé !", true);
            }
            catch (Exception e)
            {
                _flasher.Flash(Types.Danger, $"Une erreur s'est produite lors de l'envoi : {e.Message}", true);
                throw;
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}