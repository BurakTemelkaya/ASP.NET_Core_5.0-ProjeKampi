using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminMessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IBusinessUserService _userService;

        public AdminMessageController(IMessageService messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = _messageService.GetInboxWithMessageByWriter(await GetByUserID()).OrderByDescending(x => x.MessageID).ToList();
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = _messageService.GetSendBoxWithMessageByWriter(await GetByUserID()).OrderByDescending(x => x.MessageID).ToList();
            return View(values);
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return user.Id;
        }
        [HttpGet]
        public IActionResult Read(int id)
        {
            var value = _messageService.TGetByID(id);
            if (value.ReceiverUser.UserName != User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            return View(value);
        }
    }
}
