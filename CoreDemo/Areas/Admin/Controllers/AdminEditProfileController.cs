using BusinessLayer.Abstract;
using CoreDemo.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminEditProfileController : Controller
    {
        IBusinessUserService _businessUserService;
        readonly WriterCity _writerCity;
        readonly SignInManager<AppUser> _signInManager;
        public AdminEditProfileController(IBusinessUserService businessUserService, WriterCity writerCity,
            SignInManager<AppUser> signInManager)
        {
            _businessUserService = businessUserService;
            _writerCity = writerCity;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            var value = await _businessUserService.FindByUserNameAsync(User.Identity.Name);
            if (value.ImageUrl != null && value.ImageUrl[..5] != "https" || value.ImageUrl[..4] != "http")
                value.ImageUrl = null;
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserDto userDto)
        {
            var oldValue = await _businessUserService.GetByIDAsync(userDto.Id.ToString());
            var result = await _businessUserService.UpdateUserAsync(userDto);
            if (result != null)
            {
                ModelState.AddModelError("Email", "Kullanıcı bilgilerinizi güncellerken bir hata meydana geldi." +
                    " Lütfen daha sonra tekrar deneyiniz");
                ViewBag.Cities = await _writerCity.GetCityListAsync();
                return View(userDto);
            }
            var user = await _businessUserService.GetByIDAsync(userDto.Id.ToString());
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: true);
            if (user.PasswordHash == oldValue.PasswordHash && userDto.Password != null &&
                userDto.PasswordAgain != null && userDto.OldPassword != null)
            {
                ModelState.AddModelError("Password", "Parola güncellenirken bir hata oluştu lütfen değerleri düzgün girdiğinizden" +
                    "emin olunuz. Eğer Düzenlediyseniz diğer bilgileriniz güncellenmiştir.");
            }
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            return View(userDto);
        }
    }
}
