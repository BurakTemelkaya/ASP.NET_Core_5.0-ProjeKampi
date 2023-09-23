using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var blogs = await _blogService.GetListWithCategory(x => x.BlogStatus, 10);
            return View(blogs.Data);
        }
    }
}
