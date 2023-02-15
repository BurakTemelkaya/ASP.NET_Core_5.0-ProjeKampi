using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var values = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name);
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = await _messageService.GetSendBoxWithMessageListAsync(User.Identity.Name);
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var value = await _messageService.GetReceivedMessageAsync(User.Identity.Name, x => x.MessageID == id);
            if (value == null)
                value = await _messageService.GetSendMessageAsync(User.Identity.Name, x => x.MessageID == id);
            if (value == null)
                return RedirectToAction("Inbox");
            if (value.MessageStatus)
                await _messageService.MarkUsReadAsync(id, User.Identity.Name);
            return View(value);
        }

        public async Task<IActionResult> GetMessageList()
        {
            var values = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name);
            var jsonValues = JsonConvert.SerializeObject(values);
            return Json(jsonValues);
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
            await _messageService.AddMessageAsync(message, User.Identity.Name, Receiver);
            return RedirectToAction("Inbox");
        }

        [HttpPost]
        public async Task<IActionResult> MarkChanged(int id)
        {
            bool isChanged = await _messageService.MarkChangedAsync(id, User.Identity.Name);
            if (isChanged)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
