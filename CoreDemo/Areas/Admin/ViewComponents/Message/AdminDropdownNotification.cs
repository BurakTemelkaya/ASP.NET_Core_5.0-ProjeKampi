using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
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
            var notifications = await _notificationService.GetListAsync(x=> x.NotificationStatus);
            var values = await notifications.TakeLast(3).ToListAsync();
            ViewBag.NotificationCount = notifications.Count;
            return View(values);
        }
    }
}
