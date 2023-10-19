using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.ViewComponents.Message
{
    public class AdminDropdownNotification : ViewComponent
    {
        readonly INotificationService _notificationService;
        public AdminDropdownNotification(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await _notificationService.GetListAsync(x => x.NotificationStatus, 3);

            ViewBag.NotificationCount = _notificationService.GetCountAsync(x => x.NotificationDate > DateTime.Now.AddDays(30)).Result.Data;
            return View(notifications.Data);
        }
    }
}
