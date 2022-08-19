using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMessageController : Controller
    {
        private readonly IMessage2Service _message2Service;
        private readonly IBusinessUserService _userService;

        public AdminMessageController(IMessage2Service message2Service, IBusinessUserService userService)
        {
            _message2Service = message2Service;
            _userService = userService;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = _message2Service.GetInboxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        public async Task<IActionResult> SendBox()
        {
            var values = _message2Service.GetSendBoxWithMessageByWriter(await GetByUserID());
            return View(values);
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return user.Id;
        }
        public IActionResult ComposeMessage()
        {
            return View();
        }
    }
}
