﻿using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
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
            var value = await _aboutService.GetAboutByFooterAsync();
            return View(value.Data);
        }
    }
}
