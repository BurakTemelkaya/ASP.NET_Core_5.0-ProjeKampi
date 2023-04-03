using BusinessLayer.Abstract;
using CoreLayer.Utilities.CaptchaUtilities;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            string isValid = await _captchaService.RecaptchaControl(captcharesponse);
            if (isValid == null)
            {
                comment.BlogID = blogId;
                var result = await _commentService.TAddAsync(comment);
                if (result.Success)
                {
                    return Ok();
                }
                return BadRequest(result.Message);
            }
            return BadRequest("Recaptcha error.");

        }
        public async Task<PartialViewResult> CommentListByBlog(int id)
        {
            var values = await _commentService.GetListByIdAsync(id);
            return PartialView(values.Data);
        }
    }
}
