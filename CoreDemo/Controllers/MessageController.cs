using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessage2Service _message2Service;
        private readonly IBusinessUserService _userService;

        public MessageController(IMessage2Service message2Service, IBusinessUserService userService)
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
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var values = _message2Service.GetInboxWithMessageByWriter(await GetByUserID())
                .Where(x => x.MessageID == id).FirstOrDefault();
            return View(values);
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return user.Id;
        }
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message2 message, string ReceiverMail)
        {
            if (ModelState.IsValid)
            {
                message.SenderUser.Id = await GetByUserID();
                var receiver = await _userService.FindByMailAsync(ReceiverMail);
                if (receiver.Id != 0)
                {
                    message.ReceiverUser.Id = receiver.Id;
                }
                else
                {
                    return View();
                }
                message.MessageStatus = true;
                message.MessageDate = DateTime.Now;
                _message2Service.TAdd(message);
                return RedirectToAction("Inbox");
            }
            return View();
        }
    }
}
