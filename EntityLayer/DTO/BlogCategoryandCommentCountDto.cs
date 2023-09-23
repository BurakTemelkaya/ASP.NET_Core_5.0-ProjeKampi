﻿using EntityLayer.Concrete;

namespace EntityLayer.DTO
{
    public class BlogCategoryandCommentCountDto : Blog
    {
        public string CategoryName { get; set; }

        public bool CategoryStatus { get; set; }

        public bool CommentStatus { get; set; } = false;

        public int CommentCount { get; set; } = 0;

        public double CommentScore { get; set; } = 0;
    }
}
