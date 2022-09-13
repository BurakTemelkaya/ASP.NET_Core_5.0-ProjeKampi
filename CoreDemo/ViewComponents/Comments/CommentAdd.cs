using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace CoreDemo.ViewComponents.Comments
{
    public class CommentAdd : ViewComponent
    {
        ICommentService _commentService;
        public CommentAdd(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IViewComponentResult Invoke(int blogId)
        {
            ViewBag.BlogId = blogId;
            return View();
        }
    }
}
