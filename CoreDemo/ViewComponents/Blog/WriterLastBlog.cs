using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Blog
{
    public class WriterLastBlog : ViewComponent
    {
        private readonly IBlogService _blogService;

        public WriterLastBlog(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int writerId, int blogId)
        {
            var blogs = await _blogService.GetListAsync(x => x.BlogStatus && x.BlogID != blogId && x.WriterID != writerId);
            return View(blogs.Data.TakeLast(4).ToList());
        }
    }
}
