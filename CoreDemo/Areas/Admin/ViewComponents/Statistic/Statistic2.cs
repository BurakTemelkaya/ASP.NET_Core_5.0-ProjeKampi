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
            var value = await _blogService.GetListAsync();
            ViewBag.v1 = value.Select(y => y.BlogTitle).TakeLast(1).FirstOrDefault();
            ViewBag.v2 = await _blogService.GetCountAsync(x=> x.BlogStatus);
            return View();
        }
    }
}
