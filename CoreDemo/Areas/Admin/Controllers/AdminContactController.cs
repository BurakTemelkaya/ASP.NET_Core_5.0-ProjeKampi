using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetContactList()
        {
            var values = await _contactService.GetListAsync();
            values = await values.OrderByDescending(x => x.ContactID).ToListAsync();
            var jsonValues = JsonConvert.SerializeObject(values);
            return Ok(jsonValues);
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

        public async Task<IActionResult> DeleteByAjax(int id)
        {
            var value = await _contactService.TGetByIDAsync(id);
            if (value != null)
            {
                await _contactService.TDeleteAsync(value);
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> MarkReadContacts(List<string> selectedItems)
        {
            var result = await _contactService.MarksUsReadAsync(selectedItems);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> MarkUnreadContacts(List<string> selectedItems)
        {
            var result = await _contactService.MarksUsUnreadAsync(selectedItems);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        public async Task<IActionResult> MarkUsUnread(int id)
        {
            await _contactService.MarkUsUnreadAsync(id);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteContacts(List<string> selectedItems)
        {
            var result = await _contactService.DeleteContactsAsync(selectedItems);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
