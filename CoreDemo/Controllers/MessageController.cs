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
            var values = await _messageService.GetInboxWithMessageListAsync(await GetByUserID());
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = await _messageService.GetSendBoxWithMessageListAsync(await GetByUserID());
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            if (id != 0)
            {
                var value = await _messageService.GetReceivedMessageAsync(await GetByUserID(), x => x.MessageID == id);
                if (value == null)
                    value = await _messageService.GetSendMessageAsync(await GetByUserID(), x => x.MessageID == id);
                if (value == null)
                    return RedirectToAction("Inbox");
                if (value.ReceiverUserId != user.Id && value.SenderUserId != user.Id)
                    return RedirectToAction("Inbox");
                value.MessageStatus = false;
                await _messageService.TUpdateAsync(value);
                return View(value);
            }
            return RedirectToAction("Inbox");
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return user.Id;
        }
        public async Task<JsonResult> OnUserNameGet(String term)
        {
            var values = new List<AppUser>();
            if (term == null)
                values = await _userService.GetUserListAsync();
            else
                values = await _userService.GetUserListAsync(x => x.UserName.ToLower().Contains(term.ToLower()));
            var users = values.Select(x => x.UserName);
            return new JsonResult(users);
        }
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message, string Receiver)
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
            await _messageService.TAddAsync(message);
            return RedirectToAction("Inbox");
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
