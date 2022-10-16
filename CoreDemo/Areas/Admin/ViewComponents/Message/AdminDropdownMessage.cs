using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            var values = _messageService.GetInboxWithMessageList(user.Id);
            if (values.Count > 3)
                values = values.TakeLast(3).ToList();
            ViewBag.UnreadMessageCount = _messageService.GetCount(x => x.ReceiverUser.UserName == User.Identity.Name && x.MessageStatus);
            return View(values);
        }
    }
}
