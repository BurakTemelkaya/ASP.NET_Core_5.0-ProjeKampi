using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Comments
{
    public class CommentListByBlog : ViewComponent
    {
        private readonly ICommentService _commentService;

        public CommentListByBlog(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var values = await _commentService.GetListByBlogIdAsync(id);
            return View(values.Data);
        }
    }
}
