using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace CoreDemo.Controllers
{
    public class CommentByWriterController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentByWriterController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _commentService.GetCommentListByWriterandPaging(User.Identity.Name, 5, page);
            return View(result.Data.ToPagedList(page, 5));
        }

        public async Task<IActionResult> ChangeStatusComment(int id)
        {
            await _commentService.ChangeStatusCommentByWriter(User.Identity.Name, id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentByWriter(User.Identity.Name, id);
            return RedirectToAction("Index");
        }
    }
}
