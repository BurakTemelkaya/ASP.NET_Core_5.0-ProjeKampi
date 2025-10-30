using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Blog;

public class WriterLastBlog : ViewComponent
{
    private readonly IBlogService _blogService;

    public WriterLastBlog(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int writerId, int blogId)
    {
        var blogs = await _blogService.GetListByReadAllLastBlogsByWriterAsync(blogId, writerId, 4);
        return View(blogs.Data.ToList());
    }
}
