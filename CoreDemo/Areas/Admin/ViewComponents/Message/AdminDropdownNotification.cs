using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
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
            var values = await _notificationService.GetListAsync();
            return View(values);
        }
    }
}
