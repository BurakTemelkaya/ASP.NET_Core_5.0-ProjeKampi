using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterNotification : ViewComponent
    {
        readonly INotificationService _notificationService;
        public WriterNotification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _notificationService.GetListAsync(x => x.NotificationStatus == true
            && x.NotificationStatus == true);

            if (result.Success)
            {
                var values = result.Data;

                if (values.Count > 3)
                {
                    values = await values.Take(3).ToListAsync();
                }

                return View(result.Data);
            }

            return View();
        }
    }
}
