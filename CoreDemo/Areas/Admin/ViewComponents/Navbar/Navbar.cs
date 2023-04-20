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
            var unreadMessageCount = await _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name);
            if (unreadMessageCount.Success)
            {
                ViewBag.UnreadMessageCount = unreadMessageCount.Data;
            }
            return View();
        }
    }
}
