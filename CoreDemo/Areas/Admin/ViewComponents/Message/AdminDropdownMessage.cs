using BusinessLayer.Abstract;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
            var values = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name);
            if (values.Count > 3)
                values = await values.TakeLast(3).ToListAsync();
            ViewBag.UnreadMessageCount = await _messageService.GetCountAsync(x => x.ReceiverUser.UserName == User.Identity.Name && !x.MessageStatus);
            return View(values);
        }
    }
}
