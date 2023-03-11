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
        public async Task<IViewComponentResult> InvokeAsync(int writerId, int blogId)
        {
            var blogs = await _blogService.GetListAsync();
            var lastBlogs = await blogs.Where(x => x.BlogStatus).ToListAsync();
            lastBlogs.RemoveAll(x => x.WriterID == writerId);
            var currentBlog = blogs.FirstOrDefault(x => x.BlogID == blogId);
            lastBlogs.Remove(currentBlog);
            return View(await lastBlogs.TakeLast(4).ToListAsync());
        }
    }
}
