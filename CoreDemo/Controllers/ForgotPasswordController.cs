using AutoMapper;
using BusinessLayer.Abstract;
using CoreDemo.Models;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.MailUtilities;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class ForgotPasswordController : Controller
    {
        readonly IBusinessUserService _businessUserService;
        readonly SignInManager<AppUser> _signInManager;
        readonly IMailService _mailService;
        readonly ICaptchaService _captchaService;
        protected IMapper Mapper { get; }
        public ForgotPasswordController(IBusinessUserService businessUserService, SignInManager<AppUser> signInManager,
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
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                return View();
            }
            if (email != null)
            {
                var token = await _businessUserService.GetPasswordResetTokenAsync(email);
                if (token!=null)
                {
                    var callBack = Url.Action(nameof(ResetPassword), "ForgotPassword", new { token, email }, Request.Scheme);
                    _mailService.SendMail(email, MailTemplates.ResetPasswordSubject(), MailTemplates.ResetPasswordContent(callBack));
                    TempData["OkMessage"] = "Parola sıfırlama maili gönderildi.";
                    return View();
                }                
            }
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            TempData["ErrorMessage"] = "Böyle bir mail adresine sahip hesap bulunamadı.";
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (forgotPasswordModel.Token == null || forgotPasswordModel.Email == null)
            {
                Response.Redirect("/Blog/Index");
            }
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            var value = Mapper.Map<ResetPasswordDto>(forgotPasswordModel);
            return View(value);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                return View();
            }
            var isSuccess = await _businessUserService.ResetPassword(resetPasswordDto);
            if (!isSuccess)
            {
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                @TempData["ErrorMessage"] = "Mailinizin geçerlilik süresi doldu.";
                return View(resetPasswordDto);
            }
            var user = await _businessUserService.FindByMailAsync(resetPasswordDto.Email);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
