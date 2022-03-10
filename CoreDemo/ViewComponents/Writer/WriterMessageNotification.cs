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
        private readonly INotificationService _notificationService;

        public WriterMessageNotification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IViewComponentResult Invoke()
        {
            //int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value);
            //var values = _notificationService.GetInboxListByWriter(id);
            //if (values.Count() > 3)
            //{
            //    values = values.TakeLast(3).ToList();
            //}
            //ViewBag.NewMessage = values.Where(x => x.MessageStatus == true).Count();
            //return View(values);
            return View();
        }
    }
}
