using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class MessageDraftController : Controller
    {
        private readonly IMessageDraftService _messageDraftService;
        public MessageDraftController(IMessageDraftService messageDraftService)
        {
            _messageDraftService = messageDraftService;
        }
        public async Task<IActionResult> Index()
        {
            var values = await _messageDraftService.GetMessageDraftListByUserNameAsync(User.Identity.Name, null, 50);
            return View(values.Data);
        }
        [HttpGet]
        public IActionResult AddMessageDraft()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMessageDraft(MessageDraft messageDraft)
        {
            await _messageDraftService.AddAsync(messageDraft, User.Identity.Name);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditMessageDraft(int id)
        {
            var value = await _messageDraftService.GetByIDAsync(id, User.Identity.Name);
            return View(value.Data);
        }
        [HttpPost]
        public async Task<IActionResult> EditMessageDraft(MessageDraft messageDraft)
        {
            await _messageDraftService.UpdateAsync(messageDraft, User.Identity.Name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteMessageDraft(int id)
        {
            if (id != 0)
            {
                await _messageDraftService.DeleteAsync(id, User.Identity.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
