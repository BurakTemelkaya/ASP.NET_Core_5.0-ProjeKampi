using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminBlogController : Controller
    {
        readonly IBlogService _blogService;
        public AdminBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Index(int page = 1)
        {
            var values = _blogService.GetBlogListWithCategory().ToPagedList(page, 4);
            foreach (var item in values)
            {
                if (item.BlogContent.Length > 150)
                {
                    item.BlogContent = item.BlogContent.Substring(0, 130) + "...";
                }
            }
            return View(values);
        }
    }
}
