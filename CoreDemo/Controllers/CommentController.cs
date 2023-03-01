using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreLayer.Utilities.CaptchaUtilities;
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
        private readonly ICaptchaService _captchaService;

        public CommentController(ICommentService commentService, ICaptchaService captchaService)
        {
            _commentService = commentService;
            _captchaService = captchaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PartialAddComment(Comment comment, int blogId, string captcharesponse)
        {
            string isValid = await _captchaService.RecaptchaControl(HttpContext, captcharesponse);
            if (isValid == null)
            { 
                comment.BlogID = blogId;
                await _commentService.AddAsync(comment);
                return Ok();
            }
            else
            {
                return BadRequest("Recaptcha error.");
            }
        }
        public async Task<PartialViewResult> CommentListByBlog(int id)
        {
            var values = await _commentService.GetListByIdAsync(id);
            return PartialView(values);
        }
    }
}
