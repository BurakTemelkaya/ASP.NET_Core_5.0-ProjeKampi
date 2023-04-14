using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.About
{
    public class AboutComponentByFooter : ViewComponent
    {
        private readonly IAboutService _aboutService;

        public AboutComponentByFooter(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _aboutService.GetFirst();
            if (value.Data.AboutDetails1.Length > 475)
            {
                value.Data.AboutDetails1 = value.Data.AboutDetails1[..475] + "...";
            }
            return View(value.Data);
        }
    }
}
