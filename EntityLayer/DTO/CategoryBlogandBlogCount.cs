using CoreLayer.Entities;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace EntityLayer.DTO
{
    public class CategoryBlogandBlogCountDto : IDto
    {
        public Category Category { get; set; }
        public int CategoryBlogCount { get; set; }
    }
}
