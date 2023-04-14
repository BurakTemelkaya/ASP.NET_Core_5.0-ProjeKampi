using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic4 : ViewComponent
    {
        readonly IBusinessUserService _userService;
        readonly IContactService _contactService;
        readonly INotificationService _notificationService;
        readonly IBlogService _blogService;
        readonly IMessageService _messageService;
        readonly ICommentService _commentService;
        public Statistic4(IBusinessUserService userService, IContactService contactService, INotificationService notificationService
            , IBlogService blogService, IMessageService messageService, ICommentService commentService)
        {
            _userService = userService;
            _contactService = contactService;
            _notificationService = notificationService;
            _blogService = blogService;
            _messageService = messageService;
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _userService.GetByUserNameAsync(User.Identity.Name);
            if (result.Success)
            {
                var value = result.Data;

                ViewBag.v1 = value.NameSurname;
                ViewBag.v2 = value.ImageUrl;
                ViewBag.v3 = value.About;
                ViewBag.ContactCount = _contactService.GetCountAsync().Result.Data;
                ViewBag.NotificationCount = _notificationService.GetCountAsync().Result.Data;
                ViewBag.v4 = value.Email;
                ViewBag.v5 = value.City;
                ViewBag.v6 = value.RegistrationTime;
                ViewBag.BlogCount = _blogService.GetCountAsync(x => x.WriterID == value.Id).Result.Data;
                ViewBag.SendedMessageCount = _messageService.GetCountAsync(x => x.SenderUserId == value.Id).Result.Data;
                var ratingValue = await _commentService.GetBlogListWithCommentAsync();
                var ratings = ratingValue.Data.Select(x => x.BlogScore);
                int rating = 0;
                foreach (var item in ratings)
                    rating += item;
                rating /= ratings.Count();
                ViewBag.Rating = rating;
            }
            
            return View();
        }
    }
}
