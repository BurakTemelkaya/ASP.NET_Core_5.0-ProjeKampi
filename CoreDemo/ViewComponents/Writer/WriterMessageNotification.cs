using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IBusinessUserService _userService;

        public WriterMessageNotification(IMessageService messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name, null, x => !x.MessageStatus, 3);

            ViewBag.NewMessage = await _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name);

            return View(result.Data);

        }
    }
}
