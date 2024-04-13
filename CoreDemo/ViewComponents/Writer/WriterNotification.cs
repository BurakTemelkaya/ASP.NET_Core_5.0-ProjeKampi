using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var result = await _notificationService.GetListByTakeAsync(3, x => x.NotificationStatus == true);

            if (result.Success)
            {
                return View(result.Data);
            }

            return View();
        }
    }
}
