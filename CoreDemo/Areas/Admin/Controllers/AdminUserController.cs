using AutoMapper;
using BusinessLayer.Abstract;
using CoreDemo.Areas.Admin.Models;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        readonly IBusinessUserService _userService;

        public AdminUserController(IBusinessUserService userService, IMapper mapper)
        {
            _userService = userService;
            Mapper = mapper;
        }
        protected IMapper Mapper { get; }
        public async Task<IActionResult> Index(int page = 1)
        {
            var users = await _userService.GetUserListAsync();
            var values = await users.ToPagedListAsync(page, 10);
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> BannedUser(int id)
        {
            var user = await _userService.GetByIDAsync(id.ToString());
            if (user != null)
            {
                var value = Mapper.Map<BannedUserModel>(user);
                return View(value);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> BannedUser(BannedUserModel bannedUserModel)
        {
            var user = await _userService.GetByIDAsync(bannedUserModel.Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            bool result = await _userService.BannedUser(bannedUserModel.Id, bannedUserModel.BanExpirationTime, bannedUserModel.BanMessage);
            if (!result)
            {
                ModelState.AddModelError("BanExpirationTime", "İşlem yapılırken bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
                return View(user);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> BannedUserDay(int date, string id)
        {
            bool result = await _userService.BannedUser(id, DateTime.Now.AddDays(date), "");
            if (!result)
            {
                ModelState.AddModelError("BanExpirationTime", "İşlem yapılırken bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
            }
            return RedirectToAction("BannedUser");
        }
        public async Task<IActionResult> OpenBanUser(string Id)
        {
            bool result = await _userService.BanOpenUser(Id);
            if (!result)
            {
                ModelState.AddModelError("BanExpirationTime", "İşlem yapılırken bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
            }
            return RedirectToAction("BannedUser");
        }
    }
}
