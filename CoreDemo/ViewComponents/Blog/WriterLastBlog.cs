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
    public class WriterLastBlog : ViewComponent
    {
        private readonly IBlogService _blogService;

        public WriterLastBlog(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IViewComponentResult Invoke(int writerId, int blogId)
        {
            var values = _blogService.GetBlogByWriter(writerId).Where(x => x.BlogStatus).ToList();
            var currentBlog = values.FirstOrDefault(x => x.BlogID == blogId);
            values.Remove(currentBlog);
            return View(values);
        }
    }
}
