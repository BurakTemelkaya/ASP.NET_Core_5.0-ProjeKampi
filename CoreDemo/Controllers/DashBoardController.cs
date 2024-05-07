using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IUserBusinessService _userService;

        public DashBoardController(IBlogService blogService, ICategoryService categoryService, IUserBusinessService userService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var totalBlogCount = await _blogService.GetCountAsync(true);
            ViewBag.ToplamBlogSayisi = totalBlogCount.Data;
            ViewBag.YazarinBlogSayisi = _blogService.GetBlogCountByWriterAsync(User.Identity.Name).Result.Data;
            ViewBag.KategoriSayisi = _categoryService.GetCountAsync(true).Result.Data;
            return View();
        }
    }
}
