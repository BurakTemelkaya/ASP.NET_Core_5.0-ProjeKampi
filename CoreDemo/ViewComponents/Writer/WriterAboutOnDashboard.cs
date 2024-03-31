using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterAboutOnDashboard : ViewComponent
    {
        private readonly IUserBusinessService _userService;

        public WriterAboutOnDashboard(IUserBusinessService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetByUserNameAsync(User.Identity.Name);
            return View(user.Data);
        }
    }
}
