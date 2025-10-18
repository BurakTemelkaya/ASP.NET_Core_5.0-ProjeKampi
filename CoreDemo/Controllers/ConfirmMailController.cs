using BusinessLayer.Abstract;
using CoreDemo.Models;
using CoreLayer.BackgroundTasks;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers;

[AllowAnonymous]
public class ConfirmMailController : Controller
{
    private readonly IUserBusinessService _businessUserService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILoginLoggerService _loginLogger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public ConfirmMailController(IUserBusinessService businessUserService, SignInManager<AppUser> signInManager, ILoginLoggerService loginLogger, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _businessUserService = businessUserService;
        _signInManager = signInManager;
        _loginLogger = loginLogger;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public async Task<IActionResult> Index(string email, string token)
    {
        EmailErrorsModel model = new();
        if (email == null || token == null)
        {
            model.IdentityError = "Link geçersizdir.";
            return View(model);
        }

        IResultObject result = await _businessUserService.ConfirmMailAsync(email, token);
        if (result.Success)
        {
            IDataResult<UserDto> userResult = await _businessUserService.GetByMailAsync(email);
            if (userResult.Success)
            {
                await _signInManager.SignInAsync(userResult.Data, false);

                HttpContext httpContext = HttpContext;

                await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                {
                    await _loginLogger.AddAsync(userResult.Data.UserName, httpContext);
                });

                return RedirectToAction("Index", "Dashboard");
            }
        }
        model.IdentityError = result.Message;
        return View(model);
    }
}