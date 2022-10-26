using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.ViewComponents.Blog
{
    public class LastBlogs : ViewComponent
    {
        private readonly IBlogService _blogService;

        public LastBlogs(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogService.GetListAsync();
            var last3Blog = await blogs.Where(x => x.BlogStatus).TakeLast(3).ToListAsync();
            return View(last3Blog);
        }
    }
}
