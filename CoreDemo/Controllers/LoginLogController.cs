﻿using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

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
            return View(await logs.Data.ToPagedListAsync(page, 10));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var log = await _loginLoggerService.GetByUserAsync(id);
            return View(log.Data);
        }
    }
}
