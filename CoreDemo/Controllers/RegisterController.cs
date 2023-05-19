using BusinessLayer.Abstract;
using CoreDemo.Models;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.MailUtilities;
using DocumentFormat.OpenXml.Vml;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IBusinessUserService _userService;
        private readonly WriterCity _writerCity;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICaptchaService _captchaService;
        private readonly IMailService _mailService;

        public RegisterController(IBusinessUserService userService, WriterCity writerCity, SignInManager<AppUser> signInManager,
            ICaptchaService captchaService, IMailService mailService)
        {
            _userService = userService;
            _writerCity = writerCity;
            _signInManager = signInManager;
            _captchaService = captchaService;
            _mailService = mailService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpDto userSignUpDto)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            ViewBag.Cities = await _writerCity.GetCityListAsync();

            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                return View(userSignUpDto);
            }
            if (!userSignUpDto.IsAcceptTheContract)
            {
                ModelState.AddModelError("IsAcceptTheContract",
                    "Sayfamıza kayıt olabilmek için gizlilik sözleşmesini kabul etmeniz gerekmektedir.");
                return View(userSignUpDto);
            }
            var result = await _userService.RegisterUserAsync(userSignUpDto, userSignUpDto.Password);
            if (result.Success)
            {
                var tokenResult = await _userService.CreateMailTokenAsync(userSignUpDto.Email);

                string token = tokenResult.Data;

                var confirmationLink = Url.Action("Index", "ConfirmMail", new { userSignUpDto.Email, token }, Request.Scheme);

                _mailService.SendMail(userSignUpDto.Email, MailTemplates.ConfirmEmailSubject(), MailTemplates.ConfirmEmailMessage(confirmationLink));

                ViewBag.SuccessMessage = "Kayıt işlemine devam etmek için lütfen mail adresinize gelen doğrulama bağlantısına tıklayınız, teşekkürler";
                return View();
            }
            else if (result.Data != null)
            {
                foreach (var item in result.Data.Errors)
                {
                    ModelState.AddModelError("Username", item.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", result.Message);
            }
            return View(userSignUpDto);
        }
    }
}