using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var value = await _userService.FindByUserNameAsync(User.Identity.Name);
            ViewBag.v1 = value.NameSurname;
            ViewBag.v2 = value.ImageUrl;
            ViewBag.v3 = value.About;
            ViewBag.ContactCount = await _contactService.GetCountAsync();
            ViewBag.NotificationCount = await _notificationService.GetCountAsync();
            ViewBag.v4 = value.Email;
            ViewBag.v5 = value.City;
            ViewBag.v6 = value.RegistrationTime;
            ViewBag.BlogCount = await _blogService.GetCountAsync(x=> x.WriterID==value.Id);
            ViewBag.SendedMessageCount = await _messageService.GetCountAsync(x => x.SenderUserId == value.Id);
            var ratingValue = await _commentService.GetBlogListWithCommentAsync();
            var ratings = ratingValue.Select(x => x.BlogScore);
            int rating = 0;
            foreach (var item in ratings)
                rating += item;
            rating /= ratings.Count();
            ViewBag.Rating = rating;
            return View();
        }
    }
}
