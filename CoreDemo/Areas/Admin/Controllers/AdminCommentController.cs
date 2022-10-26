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
    public class AdminCommentController : Controller
    {
        readonly ICommentService _commentService;
        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var comments = await _commentService.GetBlogListWithCommentAsync();
            var values = await comments.ToPagedListAsync(page, 5);
            return View(values);
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            var value = await _commentService.TGetByIDAsync(id);
            if (value != null)
            {
                await _commentService.TDeleteAsync(value);
                ViewBag.ReturnMessage = "Yorum Başarıyla Silindi";
            }
            else
                ViewBag.ReturnMessage = "Yorumu Silerken Bir Hata Oluştu";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var value = await _commentService.TGetByIDAsync(id);
            if (value != null)
            {
                return View(value);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditComment(Comment comment)
        {
            if (comment != null)
                await _commentService.TUpdateAsync(comment);
            return RedirectToAction("Index");
        }
    }
}
