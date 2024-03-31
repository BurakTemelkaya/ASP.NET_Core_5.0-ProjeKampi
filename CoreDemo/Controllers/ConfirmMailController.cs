using BusinessLayer.Abstract;
using CoreDemo.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class ConfirmMailController : Controller
    {
        private readonly IUserBusinessService _businessUserService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILoginLoggerService _loginLogger;

        public ConfirmMailController(IUserBusinessService businessUserService, SignInManager<AppUser> signInManager, ILoginLoggerService loginLogger)
        {
            _businessUserService = businessUserService;
            _signInManager = signInManager;
            _loginLogger = loginLogger;
        }

        public async Task<IActionResult> Index(string email, string token)
        {
            EmailErrorsModel model = new();
            if (email == null || token == null)
            {
                model.IdentityError = "Link geçersizdir.";
                return View(model);
            }
            var result = await _businessUserService.ConfirmMailAsync(email, token);
            if (result.Success)
            {
                var userResult = await _businessUserService.GetByMailAsync(email);
                if (userResult.Success)
                {
                    await _signInManager.SignInAsync(userResult.Data, false);
                    await _loginLogger.AddAsync(userResult.Data.UserName);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            model.IdentityError = result.Message;
            return View(model);
        }
    }
}
