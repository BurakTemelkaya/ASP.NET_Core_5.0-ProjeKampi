using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Blog
{
    public class BlogListDashboard : ViewComponent
    {
        private readonly IBlogService _blogService;

        public BlogListDashboard(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _blogService.GetBlogListWithCategory().TakeLast(10).ToList();
            return View(values);
        }
    }
}
