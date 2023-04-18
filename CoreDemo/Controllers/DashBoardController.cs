using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IBusinessUserService _userService;

        public DashBoardController(IBlogService blogService, ICategoryService categoryService, IBusinessUserService userService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var totalBlogCount = await _blogService.GetCountAsync(x => x.BlogStatus == true);
            ViewBag.ToplamBlogSayisi = totalBlogCount.Data;
            ViewBag.YazarinBlogSayisi = _blogService.GetBlogCountByWriterAsync(User.Identity.Name).Result.Data;
            ViewBag.KategoriSayisi = _categoryService.GetCountAsync(x => x.CategoryStatus == true).Result.Data;
            return View();
        }
    }
}
