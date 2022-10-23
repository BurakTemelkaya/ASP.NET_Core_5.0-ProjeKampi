using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(int page = 1)
        {
            var values = _commentService.GetBlogListWithComment().ToPagedList(page, 5);
            return View(values);
        }
        public IActionResult DeleteComment(int id)
        {
            var value = _commentService.TGetByID(id);
            if (value != null)
            {
                _commentService.TDelete(value);
                ViewBag.ReturnMessage = "Yorum Başarıyla Silindi";
            }
            else
                ViewBag.ReturnMessage = "Yorumu Silerken Bir Hata Oluştu";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditComment(int id)
        {
            var value = _commentService.TGetByID(id);
            if (value != null)
            {
                return View(value);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditComment(Comment comment)
        {
            if (comment != null)
                _commentService.TUpdate(comment);
            return RedirectToAction("Index");
        }
    }
}
