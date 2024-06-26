﻿using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete
{
    public class Comment : IEntity
    {
        public int CommentID { get; set; }
        public string CommentUserName { get; set; }
        public string CommentTitle { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentDate { get; set; }
        public int BlogScore { get; set; }
        public bool CommentStatus { get; set; }
        public int BlogID { get; set; }
        public Blog? Blog { get; set; }
    }
}
