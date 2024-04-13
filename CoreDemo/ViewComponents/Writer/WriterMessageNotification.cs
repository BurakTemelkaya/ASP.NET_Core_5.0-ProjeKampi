using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IUserBusinessService _userService;

        public WriterMessageNotification(IMessageService messageService, IUserBusinessService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name);

            ViewBag.NewMessage = result.Data;

            return View(result.Data);

        }
    }
}
