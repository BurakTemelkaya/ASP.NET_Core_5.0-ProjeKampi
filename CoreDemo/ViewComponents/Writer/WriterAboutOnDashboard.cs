using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterAboutOnDashboard : ViewComponent
    {
        private readonly IBusinessUserService _userService;

        public WriterAboutOnDashboard(IBusinessUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.FindByUserNameAsync(User.Identity.Name);
            return View(user.Data);
        }
    }
}
