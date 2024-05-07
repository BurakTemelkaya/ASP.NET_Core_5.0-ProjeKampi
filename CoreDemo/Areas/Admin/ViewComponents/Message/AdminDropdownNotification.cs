using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
            var notifications = await _notificationService.GetListAsync(true, 3);

            ViewBag.NotificationCount = _notificationService.GetCountAsync().Result.Data;
            return View(notifications.Data);
        }
    }
}
