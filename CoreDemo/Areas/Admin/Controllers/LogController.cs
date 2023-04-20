using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _logService.GetLogListAsync(20, page);
            if (result.Success)
            {
                return View(await result.Data.ToPagedListAsync(page, 20));
            }
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _logService.GetLogByIdAsync(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return RedirectToAction("Index");
        }
    }
}
