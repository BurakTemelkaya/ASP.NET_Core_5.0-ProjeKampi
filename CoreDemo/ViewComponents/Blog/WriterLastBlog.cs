using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.ViewComponents.Blog
{
    public class WriterLastBlog : ViewComponent
    {
        private readonly IBlogService _blogService;

        public WriterLastBlog(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int writerId, int blogId)
        {
            var blogs = await _blogService.GetBlogListByWriterAsync(writerId);
            var values = await blogs.Where(x => x.BlogStatus).ToListAsync();
            var currentBlog = values.FirstOrDefault(x => x.BlogID == blogId);
            values.Remove(currentBlog);
            return View(values);
        }
    }
}
