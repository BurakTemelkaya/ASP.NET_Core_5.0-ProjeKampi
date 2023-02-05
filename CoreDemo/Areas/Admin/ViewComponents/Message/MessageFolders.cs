using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Message
{
    public class MessageFolders : ViewComponent
    {
        readonly IMessageService _messageService;
        readonly IMessageDraftService _messageDraftService;
        public MessageFolders(IMessageService messageService, IMessageDraftService messageDraftService)
        {
            _messageService = messageService;
            _messageDraftService = messageDraftService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messageCount = await _messageService.GetCountAsync(x => !x.MessageStatus && x.ReceiverUser.UserName == User.Identity.Name);
            ViewBag.ReceivedUnreadMessage = messageCount.ToString();
            var messageDraftCount = await _messageDraftService.GetCountAsync();
            ViewBag.MessageDraftCount = messageDraftCount.ToString();
            return View();
        }
    }
}
