using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminNewsLetterDraftController : Controller
    {
        readonly INewsLetterDraftService _newsLetterDraftServiceservice;
        public AdminNewsLetterDraftController(INewsLetterDraftService newsLetterDraftServiceservice)
        {
            _newsLetterDraftServiceservice = newsLetterDraftServiceservice;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var newsLetterDrafts = await _newsLetterDraftServiceservice.GetListAsync();
            var values = await newsLetterDrafts.Data.ToPagedListAsync(page, 5);
            return View(values);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(NewsLetterDraft newsLetterDraft)
        {
            await _newsLetterDraftServiceservice.TAddAsync(newsLetterDraft);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var value = await _newsLetterDraftServiceservice.TGetByIDAsync(id);
            if (value.Success)
            {
                
                return View(value.Data);
            }

            ModelState.AddModelError("", value.Message);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NewsLetterDraft newsLetterDraft)
        {
            await _newsLetterDraftServiceservice.TUpdateAsync(newsLetterDraft);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _newsLetterDraftServiceservice.DeleteById(id);
            return RedirectToAction("Index");
        }
    }
}
