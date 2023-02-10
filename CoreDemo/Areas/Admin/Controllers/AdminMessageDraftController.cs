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
            var values = await _messageDraftService.GetListAsync();
            return View(values);
        }

        public async Task<IActionResult> GetMessageDraftList()
        {
            var values = await _messageDraftService.GetListAsync();
            var jsonValues = JsonConvert.SerializeObject(values);
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
            if (id != 0)
            {
                var value = await _messageDraftService.GetByIDAsync(id, User.Identity.Name);
                return View(value);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MessageDraft messageDraft)
        {
            if (messageDraft.MessageDraftID == 0 || messageDraft.UserId == 0)
            {
                ModelState.AddModelError("Subject", "Bir hata oluştu lütfen daha sonra tekrar deneyiniz");
                return View(messageDraft);
            }
            await _messageDraftService.UpdateAsync(messageDraft, User.Identity.Name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                await _messageDraftService.DeleteAsync(id, User.Identity.Name);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessageDrafts(List<string> selectedItems)
        {
            var result = await _messageDraftService.DeleteMessageDraftsAsync(selectedItems, User.Identity.Name);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
