using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.ViewComponents.Message
{
    public class MessageFolders : ViewComponent
    {
        readonly IMessageService _messageService;
        public MessageFolders(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.ReceivedUnreadMessage = _messageService.
                GetCount(x => x.MessageStatus && x.ReceiverUser.UserName == User.Identity.Name);
            return View();
        }
    }
}
