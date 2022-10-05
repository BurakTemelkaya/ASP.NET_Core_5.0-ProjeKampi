using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{    
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminMessageController : Controller
    {
        private readonly IMessageService _message2Service;
        private readonly IBusinessUserService _userService;

        public AdminMessageController(IMessageService message2Service, IBusinessUserService userService)
        {
            _message2Service = message2Service;
            _userService = userService;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = _message2Service.GetInboxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = _message2Service.GetSendBoxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return user.Id;
        }
        [HttpGet]
        public IActionResult ComposeMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ComposeMessage(Message message)
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            message.SenderUser = user;
            message.ReceiverUser.Id = 2;
            message.MessageDate = DateTime.Now;
            _message2Service.TAdd(message);
            return RedirectToAction("SendBox");
        }
    }
}
