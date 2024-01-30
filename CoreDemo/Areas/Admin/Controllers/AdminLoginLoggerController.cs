using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminLoginLoggerController : Controller
    {
        private readonly ILoginLoggerService _loginLoggerService;

        public AdminLoginLoggerController(ILoginLoggerService loginLoggerService)
        {
            _loginLoggerService = loginLoggerService;
        }
        public async Task<IActionResult> Index(int page = 1, string userName = null)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                ViewBag.UserName = userName;
            }
            var datas = await _loginLoggerService.GetListAllAsync(page, 10, userName);
            var values = await datas.Data.ToPagedListAsync(page, 10);
            return View(values);
        }
    }
}