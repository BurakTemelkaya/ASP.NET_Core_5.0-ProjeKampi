using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.ViewComponents.Blog
{
    public class BlogListDashboard : ViewComponent
    {
        private readonly IBlogService _blogService;

        public BlogListDashboard(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogs = await _blogService.GetBlogListWithCategoryAsync();
            var values = await blogs.Data.TakeLast(10).ToListAsync();
            return View(values);
        }
    }
}
