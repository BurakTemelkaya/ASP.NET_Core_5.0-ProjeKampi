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
            var messageCountResult = await _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name);
            var messageCount = messageCountResult.Data;
            ViewBag.ReceivedUnreadMessage = messageCount;
            var messageDraftCount = await _messageDraftService.GetCountAsync();
            ViewBag.MessageDraftCount = messageDraftCount.Data.ToString();
            return View();
        }
    }
}
