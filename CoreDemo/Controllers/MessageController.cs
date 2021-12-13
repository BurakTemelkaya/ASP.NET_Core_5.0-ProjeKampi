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

        public MessageController(IMessage2Service message2Service, IWriterService writerService)
        {
            _message2Service = message2Service;
            _writerService = writerService;
        }

        public IActionResult Inbox()
        {
            var values = _message2Service.GetInboxListByWriter(GetByUserID());
            return View(values);
        }
        [HttpGet]
        public IActionResult MessageDetails(int id)
        {
            var values = _message2Service.GetInboxListByWriter(GetByUserID())
                .Where(x => x.MessageID == id).FirstOrDefault();
            return View(values);
        }
        public int GetByUserID()
        {
            return _writerService.TGetByFilter(x => x.WriterID == int.Parse(User.Identity.Name)).WriterID;
        }
    }
}
