using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic1 : ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMessage2Service _message2Service;
        private readonly ICommentService _commentService;

        public Statistic1(IBlogService blogService, IMessage2Service message2Service, ICommentService commentService)
        {
            _blogService = blogService;
            _message2Service = message2Service;
            _commentService = commentService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.v1 = _blogService.GetCount();
            ViewBag.v2 = _message2Service.GetCount();
            ViewBag.v3 = _commentService.GetCount();
            return View();
        }
    }
}
