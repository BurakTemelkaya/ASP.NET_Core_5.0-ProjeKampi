using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminMessageDraftController : Controller
    {
        readonly IMessageDraftService _messageDraftService;
        public AdminMessageDraftController(IMessageDraftService messageDraftService)
        {
            _messageDraftService = messageDraftService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _messageDraftService.GetMessageDraftListByUserNameAsync(User.Identity.Name, null, 50);
            return View(values);
        }

        public async Task<IActionResult> GetMessageDraftList()
        {
            var values = await _messageDraftService.GetMessageDraftListByUserNameAsync(User.Identity.Name);
            var jsonValues = JsonConvert.SerializeObject(values.Data);
            return Json(jsonValues);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MessageDraft messageDraft)
        {
            await _messageDraftService.AddAsync(messageDraft, User.Identity.Name);
            return Ok(messageDraft);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var value = await _messageDraftService.GetByIDAsync(id, User.Identity.Name);
            if (value.Success)
            {
                return View(value.Data);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MessageDraft messageDraft)
        {
            var result = await _messageDraftService.UpdateAsync(messageDraft, User.Identity.Name);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Subject", result.Message);
            return View(messageDraft);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _messageDraftService.DeleteAsync(id, User.Identity.Name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessageDrafts(List<string> selectedItems)
        {
            var result = await _messageDraftService.DeleteMessageDraftsAsync(selectedItems, User.Identity.Name);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }
    }
}
