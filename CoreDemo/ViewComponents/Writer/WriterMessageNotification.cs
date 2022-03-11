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
        private readonly IUserService _userService;

        public WriterMessageNotification(IMessage2Service messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetByUserNameAsync(User.Identity.Name);
            var values = _messageService.GetList(x => x.ReceiverID == user.Id);
            if (values.Count() > 3)
            {
                values = values.TakeLast(3).ToList();
            }
            ViewBag.NewMessage = values.Where(x => x.MessageStatus == true).Count();
            return View(values);
        }
    }
}
