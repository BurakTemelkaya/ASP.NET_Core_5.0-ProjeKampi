using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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

        public async Task<IActionResult> Index()
        {
            var values = await _contactService.GetListAsync();
            values = await values.OrderByDescending(x => x.ContactID).ToListAsync();
            return View(values);
        }
        public async Task<IActionResult> Read(int id)
        {
            var value = await _contactService.TGetByIDAsync(id);
            if (value != null)
            {
                if (!value.ContactStatus)
                {
                    value.ContactStatus = true;
                    await _contactService.TUpdateAsync(value);
                }
                return View(value);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _contactService.TGetByIDAsync(id);
            if (value != null)
            {
                await _contactService.TDeleteAsync(value);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> MarkUsUnread(int id)
        {
            var value = await _contactService.TGetByIDAsync(id);
            if (value != null)
            {
                value.ContactStatus = true;
                await _contactService.TUpdateAsync(value);
            }
            return RedirectToAction("Index");
        }
    }
}
