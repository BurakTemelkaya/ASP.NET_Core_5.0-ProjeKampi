using CoreLayer.Entities;
using EntityLayer.Concrete;

namespace EntityLayer.DTO
{
    public class CategoryBlogandBlogCountDto : Category, IDto
    {
        public int NumberofBloginCategory { get; set; }
        public bool BlogStatus { get; set; }
    }
}
