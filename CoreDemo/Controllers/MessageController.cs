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
        private readonly IMessageService _messageService;
        private readonly IBusinessUserService _userService;

        public MessageController(IMessageService messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = _messageService.GetInboxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = _messageService.GetSendBoxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var values = _messageService.GetInboxWithMessageByWriter(await GetByUserID())
                .Where(x => x.MessageID == id).FirstOrDefault();
            values.MessageStatus = false;
            _messageService.TUpdate(values);
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
        public async Task<IActionResult> SendMessage(Message message, string Receiver)
        {
            if (ModelState.IsValid)
            {
                var sender = await _userService.FindByUserNameAsync(User.Identity.Name);
                message.SenderUser.Id = sender.Id;
                var mailUser = await _userService.FindByMailAsync(Receiver);
                var userNameUser = await _userService.FindByUserNameAsync(Receiver);

                if (mailUser != null)
                {
                    message.ReceiverUser.Id = mailUser.Id;
                }
                else if (userNameUser != null)
                {
                    message.ReceiverUser.Id = userNameUser.Id;
                }
                else
                {
                    ModelState.AddModelError("Receiver", "Girdiğiniz gönderici bilgileri bulunamadı.");
                    return View();
                }
                message.MessageStatus = true;
                message.MessageDate = DateTime.Now;
                _messageService.TAdd(message);
                return RedirectToAction("Inbox");
            }
            return View();
        }
        /// <summary>
        /// Id değeri kontrolü
        /// Mesaj kullanıcının mesajı mı kontrolü
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            bool isChanged = await _messageService.MarkChangedAsync(id, User.Identity.Name);
            if (isChanged)
            {
                return Ok();
            }
            return NotFound();
        }
        public async Task<IActionResult> MarkUsUnreadInbox(int id)
        {
            bool isChanged = await _messageService.MarkChangedAsync(id, User.Identity.Name);
            return RedirectToAction("Inbox");
        }
    }
}
