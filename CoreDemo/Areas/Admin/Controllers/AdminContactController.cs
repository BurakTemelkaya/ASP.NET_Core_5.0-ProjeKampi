using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminContactController : Controller
    {
        readonly IContactService _contactService;
        public AdminContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            var values = _contactService.GetList().OrderByDescending(x => x.ContactID).ToList();
            return View(values);
        }
        public IActionResult Read(int id)
        {
            var value = _contactService.TGetByID(id);
            if (value != null)
            {
                if (!value.ContactStatus)
                {
                    value.ContactStatus = true;
                    _contactService.TUpdate(value);
                }
                return View(value);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var value = _contactService.TGetByID(id);
            if (value != null)
            {
                _contactService.TDelete(value);
            }
            return RedirectToAction("Index");
        }
        public IActionResult MarkUsUnread(int id)
        {
            var value = _contactService.TGetByID(id);
            if (value != null)
            {
                value.ContactStatus = true;
                _contactService.TUpdate(value);
            }
            return RedirectToAction("Index");
        }
    }
}
