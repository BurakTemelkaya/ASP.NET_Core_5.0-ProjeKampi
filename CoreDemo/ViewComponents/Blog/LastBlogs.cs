using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
namespace CoreDemo.ViewComponents.Blog
{
    public class LastBlogs : ViewComponent
    {
        private readonly IBlogService _blogService;

        public LastBlogs(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int writerId, int blogId)
        {
            var blogs = await _blogService.GetListByReadAllLastBlogsAsync(blogId, writerId, 4);
            return View(blogs.Data);
        }
    }
}
