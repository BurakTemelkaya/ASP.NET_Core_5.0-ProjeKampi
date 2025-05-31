using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers;

public class BlogViewController : Controller
{
    private readonly IBlogViewService _blogViewService;

    public BlogViewController(IBlogViewService blogViewService)
    {
        _blogViewService = blogViewService;
    }

    public async Task<IActionResult> GetChartData()
    {
        var data = await _blogViewService.GetChartDataByWriterAsync();
        return Ok(data);
    }

    public async Task<IActionResult> GetChartDataByBlog(int blogId)
    {
        var data = await _blogViewService.GetChartDataByBlogId(blogId:blogId);
        return Ok(data);
    }
}
