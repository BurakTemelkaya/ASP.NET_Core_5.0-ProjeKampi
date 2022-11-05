using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Threading.Tasks;
using BusinessLayer.Models;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminNewsletterController : Controller
    {
        readonly INewsLetterService _newsLetterService;
        public AdminNewsletterController(INewsLetterService newsLetterService)
        {
            _newsLetterService = newsLetterService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var newsletter = await _newsLetterService.GetListAsync();
            var value = await newsletter.ToPagedListAsync(page, 5);
            return View(value);
        }
        [HttpGet]
        public IActionResult SendNewsletter()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendNewsletter(NewsLetterSendMailsModel model)
        {
            bool isSend = await _newsLetterService.SendMailAsync(model, x => x.MailStatus);
            if (!isSend)
            {
                ModelState.AddModelError("Subject", "Bir hata oluştu lütfen daha sonra tekrar deneyiniz.");
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
            await _newsLetterService.TDeleteAsync(value);
            return RedirectToAction("Index");
        }
    }
}
