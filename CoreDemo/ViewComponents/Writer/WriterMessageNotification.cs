using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        MessageManager notificationManager = new MessageManager(new EfMessageRepository());
        public IViewComponentResult Invoke()
        {
            var values = notificationManager.
                GetList(x => x.ReceiverMail == "temelkayaburak@gmail.com" &&
                x.MessageStatus == true && x.MessageDate.Day == DateTime.Now.Day);
            if (values.Count() > 3)
            {
                values = values.Take(3).ToList();
            }
            return View(values);
        }
    }
}
