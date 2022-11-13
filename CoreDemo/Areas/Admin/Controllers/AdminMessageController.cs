using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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

        public async Task<IActionResult> Inbox(string search = null)
        {
            List<Message> values = new();
            if (search != null)
            {
                values = await _messageService.GetInboxWithMessageListAsync(await GetByUserID(),
                    x => x.Subject.ToLower().Contains(search.ToLower()));
                values = await values.OrderByDescending(x => x.MessageID).ToListAsync();
            }
            if (values.Count == 0)
            {
                values = await _messageService.GetInboxWithMessageListAsync(await GetByUserID());
                values = await values.OrderByDescending(x => x.MessageID).ToListAsync();
            }
            return View(values);
        }
        public async Task<IActionResult> SendBox(string search = null)
        {
            List<Message> values = new();
            if (search != null)
            {
                values = await _messageService.GetSendBoxWithMessageListAsync(await GetByUserID(),
                x => x.Subject.ToLower().Contains(search.ToLower()));
                values = await values.OrderByDescending(x => x.MessageID).ToListAsync();
            }
            if (values.Count == 0)
            {
                values = await _messageService.GetSendBoxWithMessageListAsync(await GetByUserID());
                values = await values.OrderByDescending(x => x.MessageID).ToListAsync();
            }
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
        public async Task<IActionResult> SendMessage(Message message, string receiver)
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            message.SenderUserId = user.Id;
            var userNameUser = await _userService.FindByUserNameAsync(receiver);
            var mailUser = await _userService.FindByMailAsync(receiver);
            if (userNameUser != null)
                message.ReceiverUserId = userNameUser.Id;
            else if (mailUser != null)
                message.ReceiverUserId = mailUser.Id;
            else
            {
                ModelState.AddModelError("Receiver", "Girdiğiniz gönderici bilgileri bulunamadı.");
                return View(message);
            }
            await _messageService.TAddAsync(message);
            return RedirectToAction("SendBox");
        }
        public async Task<IActionResult> Read(int id)
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            var value = await _messageService.GetReceivedMessageAsync(user.Id, x => x.MessageID == id);
            if (value == null)
                value = await _messageService.GetSendMessageAsync(user.Id, x => x.MessageID == id);
            if (value == null)
                return RedirectToAction("Inbox");
            if (value.ReceiverUserId != user.Id && value.SenderUserId != user.Id)
                return RedirectToAction("Inbox");
            if (value.MessageStatus)
                await _messageService.MarkChangedAsync(id, user.UserName);
            return View(value);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            var message = await _messageService.GetReceivedMessageAsync(user.Id, x => x.MessageID == id);
            if (id != 0 && message.ReceiverUserId == user.Id)
               await _messageService.TDeleteAsync(message);
            return RedirectToAction("Inbox");
        }
    }
}
