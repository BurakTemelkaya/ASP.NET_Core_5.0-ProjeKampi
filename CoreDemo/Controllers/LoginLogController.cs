using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace CoreDemo.Controllers
{
    public class LoginLogController : Controller
    {
        private readonly ILoginLoggerService _loginLoggerService;
        public LoginLogController(ILoginLoggerService loginLoggerService)
        {
            _loginLoggerService = loginLoggerService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var logs = await _loginLoggerService.GetListByUserAsync(page, 10);
            return View(logs.Data.ToPagedList(page, 10));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var log = await _loginLoggerService.GetByUserAsync(id);
            return View(log.Data);
        }
    }
}
