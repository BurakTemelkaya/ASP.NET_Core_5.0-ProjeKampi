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
        private readonly IBusinessUserService _businessUserService;
        private readonly SignInManager<AppUser> _signInManager;

        public ConfirmMailController(IBusinessUserService businessUserService,SignInManager<AppUser> signInManager)
        {
            _businessUserService = businessUserService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string email, string token)
        {
            var result = await _businessUserService.ConfirmMailAsync(email, token);
            if (result.Success)
            {
                var userResult = await _businessUserService.GetByMailAsync(email);
                if (userResult.Success)
                {
                    await _signInManager.SignInAsync(userResult.Data, false);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            var model = new EmailErrorsModel { IdentityError = result.Message };
            return View(model);
        }
    }
}
