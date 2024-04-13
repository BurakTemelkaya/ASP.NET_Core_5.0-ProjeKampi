using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var result = await _contactService.GetListAsync();
            var values = result.Data.OrderByDescending(x => x.ContactID).ToList();
            var jsonValues = JsonConvert.SerializeObject(values);
            return Ok(jsonValues);
        }

        public async Task<IActionResult> Read(int id)
        {
            var result = await _contactService.TGetByIDAsync(id);
            if (result.Success)
            {
                var value = result.Data;
                if (!value.ContactStatus)
                {
                    await _contactService.MarkUsReadAsync(id);
                }
                return View(value);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactService.TGetByIDAsync(id);
            if (result.Success)
            {
                await _contactService.TDeleteAsync(result.Data);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteByAjax(int id)
        {
            var result = await _contactService.TGetByIDAsync(id);
            if (result.Success)
            {
                await _contactService.TDeleteAsync(result.Data);
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> MarkReadContacts(List<string> selectedItems)
        {
            var result = await _contactService.MarksUsReadAsync(selectedItems);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> MarkUnreadContacts(List<string> selectedItems)
        {
            var result = await _contactService.MarksUsUnreadAsync(selectedItems);

            if (result.Success)
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

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
