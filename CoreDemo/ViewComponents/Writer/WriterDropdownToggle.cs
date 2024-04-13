using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterDropdownToggle : ViewComponent
    {
        private readonly IUserBusinessService _userService;

        public WriterDropdownToggle(IUserBusinessService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var writer = await _userService.GetByUserNameAsync(User.Identity.Name);
            return View(writer.Data);
        }
    }
}
