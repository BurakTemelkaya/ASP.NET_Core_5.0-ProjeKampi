using AutoMapper;
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
    public class ForgotPasswordController : Controller
    {
        readonly IUserBusinessService _businessUserService;
        readonly SignInManager<AppUser> _signInManager;
        readonly IMailService _mailService;
        readonly ICaptchaService _captchaService;
        protected IMapper Mapper { get; }
        public ForgotPasswordController(IUserBusinessService businessUserService, SignInManager<AppUser> signInManager,
            IMapper mapper, IMailService mailService, ICaptchaService captchaService)
        {
            _businessUserService = businessUserService;
            _signInManager = signInManager;
            Mapper = mapper;
            _mailService = mailService;
            _captchaService = captchaService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                return View();
            }
            if (email != null)
            {
                var tokenResult = await _businessUserService.GetPasswordResetTokenAsync(email);
                if (tokenResult.Success)
                {
                    var callBack = Url.Action(nameof(ResetPassword), "ForgotPassword", new { token = tokenResult.Data, email }, Request.Scheme);
                    _mailService.SendMail(email, MailTemplates.ResetPasswordSubject(), MailTemplates.ResetPasswordContent(callBack));
                    TempData["OkMessage"] = "Parola sıfırlama maili gönderildi.";
                    return View();
                }
            }
            TempData["ErrorMessage"] = "Böyle bir mail adresine sahip hesap bulunamadı.";
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (forgotPasswordModel.Token == null || forgotPasswordModel.Email == null)
            {
                @TempData["ErrorMessage"] = "Geçersiz link girdiniz.";
            }

            var value = Mapper.Map<ResetPasswordDto>(forgotPasswordModel);

            TempData["SiteKey"] = _captchaService.GetSiteKey();

            return View(value);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            TempData["SiteKey"] = _captchaService.GetSiteKey();
            string captchaMessage = await _captchaService.RecaptchaControl();
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                return View();
            }
            var result = await _businessUserService.ResetPassword(resetPasswordDto);
            if (!result.Success)
            {
                @TempData["ErrorMessage"] = result.Message;
                return View();
            }
            var user = await _businessUserService.GetByMailAsync(resetPasswordDto.Email);

            await _signInManager.SignInAsync(user.Data, true);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
