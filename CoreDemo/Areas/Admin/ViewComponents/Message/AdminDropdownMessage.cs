using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Message
{
    public class AdminDropdownMessage : ViewComponent
    {
        readonly IMessageService _messageService;
        readonly IBusinessUserService _userService;
        public AdminDropdownMessage(IMessageService messageService, IBusinessUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name, null, null, 3);
            if (result.Success)
            {
                var values = result.Data;
                ViewBag.UnreadMessageCount = _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name).Result.Data;
                return View(values);
            }

            return View();
        }
    }
}
