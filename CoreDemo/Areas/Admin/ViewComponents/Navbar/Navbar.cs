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
            var user = await _businessUserService.FindByUserNameAsync(User.Identity.Name);
            ViewBag.MessageCount = _messageService.GetCount(x => x.ReceiverUserId == user.Id);
            ViewBag.UnreadMessageCount = _messageService.GetCount(x => x.ReceiverUserId == user.Id && x.MessageStatus);
            return View();
        }
    }
}
