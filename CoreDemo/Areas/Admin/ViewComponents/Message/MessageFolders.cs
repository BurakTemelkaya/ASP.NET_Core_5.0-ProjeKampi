﻿using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Message
{
    public class MessageFolders : ViewComponent
    {
        readonly IMessageService _messageService;
        public MessageFolders(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messageCount = await _messageService.GetCountAsync(x => x.MessageStatus && x.ReceiverUser.UserName == User.Identity.Name);
            ViewBag.ReceivedUnreadMessage = messageCount.ToString();
            return View();
        }
    }
}