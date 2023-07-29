using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Blog
{
    public class BlogLastPostByFooter : ViewComponent
    {
        private readonly IBlogService _blogService;

        public BlogLastPostByFooter(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogService.GetLastBlogAsync(4, 12);
            return View(blogs.Data);
        }
    }
}
