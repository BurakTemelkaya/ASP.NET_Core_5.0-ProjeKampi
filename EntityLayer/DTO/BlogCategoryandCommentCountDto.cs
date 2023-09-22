using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTO
{
    public class BlogCategoryandCommentCountDto : Blog
    {
        public string CategoryName { get; set; }

        public bool CategoryStatus { get; set; }

        public int CommentCount { get; set; } = 0;

        public double CommentScore { get; set; } = 0;
    }
}
