using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreDemo.Models;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.MailUtilities;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Cities = _writerCity.GetCityList();
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpDto userSignUpDto)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            ViewBag.Cities = _writerCity.GetCityList();

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
                var emailResult = await SendConfirmationUrl(userSignUpDto.Email);

                if (string.IsNullOrEmpty(emailResult))
                {
                    ViewBag.SuccessMessage = "Kayıt işlemine devam etmek için lütfen mail adresinize gelen doğrulama bağlantısına tıklayınız, teşekkürler";
                    return View();
                }
                ModelState.AddModelError("Username", emailResult);
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

        private async Task<string> SendConfirmationUrl(string email)
        {
            var tokenResult = await _userService.CreateMailTokenAsync(email);

            if (!tokenResult.Success)
            {
                return tokenResult.Message;
            }

            string token = tokenResult.Data;

            var confirmationLink = Url.Action("Index", "ConfirmMail", new { email, token }, Request.Scheme);

            _mailService.SendMail(email, MailTemplates.ConfirmEmailSubject(), MailTemplates.ConfirmEmailMessage(confirmationLink));

            return string.Empty;
        }

        [HttpGet]
        public IActionResult ReSendConfirmationMail()
        {
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReSendConfirmationMail(string email)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            ViewBag.SiteKey = _captchaService.GetSiteKey();

            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                return View(email);
            }

            var emailResult = await SendConfirmationUrl(email);

            if (string.IsNullOrEmpty(emailResult))
            {
                ViewBag.IsSend = true;
                return View();
            }
            ViewBag.ErrorMessage = emailResult;
            return View(model: email);
        }
    }
}