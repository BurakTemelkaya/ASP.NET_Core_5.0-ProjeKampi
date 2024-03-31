using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    
    public class WriterNavbarProfile : ViewComponent
    {
       readonly IUserBusinessService _businessUserService;
        readonly UserManager<AppUser> _userManager;
        public WriterNavbarProfile(IUserBusinessService businessUserService, UserManager<AppUser> userManager)
        {
            _businessUserService = businessUserService;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var writer = await _businessUserService.GetByUserNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(writer.Data);
            ViewBag.Role = roles.FirstOrDefault();
            return View(writer.Data);
        }
    }
}
