using CoreLayer.Entities;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace CoreDemo.Models
{
    public class CategoryBlogandBlogCountDto : IDto
    {
        public Blog Blog { get; set; }
        public Category Category { get; set; }
        public int CategoryCount { get; set; }
    }
}
