using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace CoreDemo.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class BlogViewController : Controller
{
    private readonly IBlogViewService _blogViewService;

    public BlogViewController(IBlogViewService blogViewService)
    {
        _blogViewService = blogViewService;
    }

    public async Task<IActionResult> Index(int page = 1, bool? isRedirect = null)
    {
        ViewBag.IsRedirect = isRedirect;
        var result = await _blogViewService.GetListByPagingNameAsync(page, 15, isRedirect);
        return View(result.Data);
    }

    public async Task<IActionResult> GetBlogViewChartData(TimeSpan? interval, DateTime? startDate, DateTime? endDate)
    {
        var result = await _blogViewService.GetChartDataByAdminAsync(interval, startDate, endDate);
        return Ok(result);
    }
}
