using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var categories = await _categoryService.GetListAsync();
            var values = await categories.Data.ToPagedListAsync(page, 5);
            return View(values);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _categoryService.TAddAsync(category);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangedStatus(int id)
        {
            await _categoryService.ChangedStatusAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var value = await _categoryService.TGetByIDAsync(id);
            await _categoryService.TDeleteAsync(value.Data);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var value = await _categoryService.TGetByIDAsync(id);
            if (value == null)
                return RedirectToAction("Index");
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            await _categoryService.TUpdateAsync(category);
            return RedirectToAction("Index");
        }
    }

}
