using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Navbar
{
    public class Navbar : ViewComponent
    {
        readonly IBusinessUserService _businessUserService;
        readonly IMessageService _messageService;
        public Navbar(IMessageService messageService, IBusinessUserService businessUserService)
        {
            _messageService = messageService;
            _businessUserService = businessUserService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _businessUserService.GetByUserNameAsync(User.Identity.Name);

            var messageCount = await _messageService.GetCountAsync(x => x.ReceiverUserId == user.Data.Id);
            if (messageCount.Success)
            {
                ViewBag.MessageCount = messageCount.Data;
            }

            var unreadMessageCount = await _messageService.GetCountAsync(x => x.ReceiverUserId == user.Data.Id && x.MessageStatus);
            if (unreadMessageCount.Success)
            {
                ViewBag.UnreadMessageCount = unreadMessageCount.Data;
            }
            return View();
        }
    }
}
