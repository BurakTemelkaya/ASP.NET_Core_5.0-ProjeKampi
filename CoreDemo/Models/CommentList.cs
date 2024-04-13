using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoreDemo.Models
{
    public class CommentList : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var commentValues = new List<UserComment>
            {
                new UserComment
                {
                    ID=1,
                    UserName="Burak"
                },
                new UserComment
                {
                    ID=2,
                    UserName="Murat"
                },
                new UserComment
                {
                    ID=3,
                    UserName="Merve"
                }
            };
            return View(commentValues);
        }
    }
}
