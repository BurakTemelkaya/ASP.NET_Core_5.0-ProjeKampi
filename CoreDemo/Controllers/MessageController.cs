using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessage2Service _message2Service;
        private readonly IWriterService _writerService;
        private readonly IBusinessUserService _userService;

        public MessageController(IMessage2Service message2Service, IWriterService writerService, IBusinessUserService userService)
        {
            _message2Service = message2Service;
            _writerService = writerService;
            _userService = userService;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = _message2Service.GetInboxListByWriter(await GetByUserID());
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> MessageDetails(int id)
        {
            var values = _message2Service.GetInboxListByWriter(await GetByUserID())
                .Where(x => x.MessageID == id).FirstOrDefault();
            return View(values);
        }
        public async Task<int> GetByUserID()
        {
            var user = await _userService.FindUserNameAsync(User.Identity.Name);
            return user.Id;
        }
    }
}
