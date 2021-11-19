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
        Message2Manager message2Manager = new Message2Manager(new EfMessage2Repository());
        public IActionResult Inbox()
        {
            var values = message2Manager.GetInboxListByWriter(1);
            return View(values);
        }
        [HttpGet]
        public IActionResult MessageDetails(int id)
        {
            var values = message2Manager.GetInboxListByWriter(1)
                .Where(x => x.MessageID == id).FirstOrDefault();
            return View(values);
        }
    }
}
