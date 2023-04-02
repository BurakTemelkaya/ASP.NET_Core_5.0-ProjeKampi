using BusinessLayer.Abstract;
using CoreLayer.Utilities.CaptchaUtilities;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ICaptchaService _captchaService;

        public ContactController(IContactService contactService, ICaptchaService captchaService)
        {
            _contactService = contactService;
            _captchaService = captchaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Contact contact)
        {
            string validationMessage = await _captchaService.RecaptchaControl();
            if (!string.IsNullOrEmpty(validationMessage))
            {
                ModelState.AddModelError("Recaptcha", validationMessage);
                return View();
            }

            await _contactService.ContactAddAsync(contact);
            return RedirectToAction("Index");
        }
    }
}
