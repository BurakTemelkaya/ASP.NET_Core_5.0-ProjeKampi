using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class BlogViewController : Controller
    {
        private readonly IBlogViewService _blogViewService;

        public BlogViewController(IBlogViewService blogViewService)
        {
            _blogViewService = blogViewService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _blogViewService.GetListByPagingNameAsync(15, page);
            return View(result.Data.ToPagedList(page,15));
        }
    }
}
