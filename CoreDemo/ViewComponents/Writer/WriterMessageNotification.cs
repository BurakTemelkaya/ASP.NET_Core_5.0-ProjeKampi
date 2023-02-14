﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IBusinessUserService _userService;

        public WriterMessageNotification(IMessageService messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name);
            if (values.Count > 3)
                values = await values.TakeLast(3).ToListAsync();
            ViewBag.NewMessage = values.Where(x => x.MessageStatus).Count();
            return View(values);

        }
    }
}
