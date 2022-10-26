using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult PartialAddComment()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PartialAddComment(Comment comment, int blogId)
        {
            comment.CommentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            comment.CommentStatus = true;
            comment.BlogID = blogId;
            await _commentService.TAddAsync(comment);
            return Ok();
        }
        public async Task<PartialViewResult> CommentListByBlog(int id)
        {
            var values = await _commentService.GetListByIdAsync(id);
            return PartialView(values);
        }
    }
}
