using BusinessLayer.Abstract;
using CoreLayer.Utilities.CaptchaUtilities;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Comments
{
    public class CommentAdd : ViewComponent
    {
        private readonly ICommentService _commentService;
        private readonly ICaptchaService _captchaService;
        public CommentAdd(ICommentService commentService, ICaptchaService captchaService)
        {
            _commentService = commentService;
            _captchaService = captchaService;
        }

        public IViewComponentResult Invoke(int blogId)
        {
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
    }
}
