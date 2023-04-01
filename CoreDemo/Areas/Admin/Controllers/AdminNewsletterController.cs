using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Threading.Tasks;
using BusinessLayer.Models;
using AutoMapper;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminNewsletterController : Controller
    {
        readonly INewsLetterService _newsLetterService;
        readonly INewsLetterDraftService _newsLetterDraftService;
        readonly IMapper _mapper;
        public AdminNewsletterController(INewsLetterService newsLetterService, INewsLetterDraftService newsLetterDraftService, IMapper mapper)
        {
            _newsLetterService = newsLetterService;
            _newsLetterDraftService = newsLetterDraftService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var newsletter = await _newsLetterService.GetListAsync();
            var value = await newsletter.Data.ToPagedListAsync(page, 5);
            return View(value);
        }

        [HttpGet]
        public async Task<IActionResult> SendNewsletter(int id)
        {
            if (id != 0)
            {
                var value = await _newsLetterDraftService.TGetByIDAsync(id);
                if (value != null)
                {
                    var newsLetter = _mapper.Map<NewsLetterSendMailsModel>(value);
                    return View(newsLetter);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNewsletter(NewsLetterSendMailsModel model)
        {
            var result = await _newsLetterService.SendMailAsync(model, x => x.MailStatus);
            if (!result.Success)
            {
                ModelState.AddModelError("Subject", result.Message);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditNewsletter(int id)
        {
            var value = await _newsLetterService.TGetByIDAsync(id);
            return value == null ? RedirectToAction("Index") : View(value);
        }

        [HttpPost]
        public async Task<IActionResult> EditNewsletter(NewsLetter newsLetter)
        {
            await _newsLetterService.TUpdateAsync(newsLetter);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteNewsLetter(int id)
        {
            var value = await _newsLetterService.TGetByIDAsync(id);
            await _newsLetterService.TDeleteAsync(value.Data);
            return RedirectToAction("Index");
        }
    }
}
