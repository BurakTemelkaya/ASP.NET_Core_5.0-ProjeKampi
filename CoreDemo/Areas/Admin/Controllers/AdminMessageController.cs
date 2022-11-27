﻿using AutoMapper;
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
        private readonly IMessageDraftService _messageDraftService;
        private readonly IBusinessUserService _userService;
        private readonly IMapper _mapper;

        public AdminMessageController(IMessageService messageService, IBusinessUserService userService, IMessageDraftService messageDraftService,
            IMapper mapper)
        {
            _messageService = messageService;
            _userService = userService;
            _messageDraftService = messageDraftService;
            _mapper = mapper;
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
        public async Task<IActionResult> SendMessage(int id, string ReceiverUser = null)
        {
            if (id != 0)
            {
                var value = await _messageDraftService.GetByIDAsync(id, User.Identity.Name);
                if (value != null)
                {
                    var message = _mapper.Map<Message>(value);
                    return View(message);
                }
            }
            if (ReceiverUser != null)
            {
                ViewBag.ReceiverUser = ReceiverUser;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message, string receiver)
        {
            await _messageService.AddMessageAsync(message, User.Identity.Name, receiver);
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
            if (id != 0)
                await _messageService.DeleteMessageAsync(id, User.Identity.Name);
            return RedirectToAction("Inbox");
        }
    }
}
