using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic2
{
    public class Statistic2 : ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMessageService _message2Service;
        private readonly ICommentService _commentService;

        public Statistic2(IBlogService blogService, IMessageService message2Service, ICommentService commentService)
        {
            _blogService = blogService;
            _message2Service = message2Service;
            _commentService = commentService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lastBlog = await _blogService.GetLastBlogAsync(1);

            var blogCount = await _blogService.GetCountAsync(x => x.BlogStatus);

            ViewBag.v1 = lastBlog.Data.First().BlogTitle;
            ViewBag.v2 = blogCount.Data;

            return View();
        }
    }
}
