using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        private readonly IMessage2Service _messageService;
        private readonly IBusinessUserService _userService;

        public WriterMessageNotification(IMessage2Service messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            var values = _messageService.GetInboxWithMessageByWriter(user.Id);
            if (values.Count() > 3)
            {
                values = values.TakeLast(3).ToList();
            }
            ViewBag.NewMessage = values.Where(x => x.MessageStatus == true).Count();
            return View(values);
        }
    }
}
