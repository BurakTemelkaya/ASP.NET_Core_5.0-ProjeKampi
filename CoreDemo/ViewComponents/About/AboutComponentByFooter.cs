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
            var blogs = await _aboutService.GetListAsync();
            var value = blogs.Data.First();
            if (value.AboutDetails1.Length > 475)
            {
                value.AboutDetails1 = value.AboutDetails1.Substring(0, 475) + "...";
            }
            return View(value);
        }
    }
}
