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
        private readonly IMessage2Service _message2Service;
        private readonly ICommentService _commentService;

        public Statistic2(IBlogService blogService, IMessage2Service message2Service, ICommentService commentService)
        {
            _blogService = blogService;
            _message2Service = message2Service;
            _commentService = commentService;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.v1 = _blogService.GetList().OrderByDescending(x => x.BlogID).Select(y => y.BlogTitle).Take(1).FirstOrDefault();
            ViewBag.v2 = _commentService.GetCount();
            return View();
        }
    }
}
