using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var values = _messageService.GetInboxWithMessageList(await GetByUserID());
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = _messageService.GetSendBoxWithMessageList(await GetByUserID());
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var user = _userService.FindByUserNameAsync(User.Identity.Name);
            if (id != 0)
            {
                var value = _messageService.GetInboxWithMessageList(await GetByUserID(), x => x.MessageID == id).FirstOrDefault();
                if (value == null)
                    value = _messageService.GetSendBoxWithMessageList(await GetByUserID(), x => x.MessageID == id).FirstOrDefault();
                if (value == null)
                    return RedirectToAction("Inbox");
                if (value.ReceiverUserId != user.Id && value.SenderUserId != user.Id)
                    return RedirectToAction("Inbox");
                value.MessageStatus = false;
                _messageService.TUpdate(value);

                return View(value);
            }
            return RedirectToAction("Inbox");
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
                message.SenderUserId = sender.Id;
                var mailUser = await _userService.FindByMailAsync(Receiver);
                var userNameUser = await _userService.FindByUserNameAsync(Receiver);

                if (mailUser != null)
                    message.ReceiverUserId = mailUser.Id;
                else if (userNameUser != null)
                    message.ReceiverUserId = userNameUser.Id;
                else
                {
                    ModelState.AddModelError("Receiver", "Girdiğiniz gönderici bilgileri bulunamadı.");
                    return View(message);
                }
                message.MessageStatus = true;
                message.MessageDate = DateTime.Now;
                _messageService.TAdd(message);
                return RedirectToAction("Inbox");
            }
            return View(message);
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
