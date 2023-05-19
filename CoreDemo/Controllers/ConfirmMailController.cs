using BusinessLayer.Abstract;
using CoreDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class ConfirmMailController : Controller
    {
        private readonly IBusinessUserService _businessUserService;

        public ConfirmMailController(IBusinessUserService businessUserService)
        {
            _businessUserService = businessUserService;
        }

        public async Task<IActionResult> Index(string email, string token)
        {
            var result = await _businessUserService.ConfirmMailAsync(email, token);
            if (result.Success)
            {
                return RedirectToAction("Dashboard", "Index");
            }
            var model = new EmailErrorsModel { IdentityError = result.Message };
            return View(model);
        }
    }
}
