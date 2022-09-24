using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace CoreDemo.Models
{
    public class CategoryBlogandBlogCount
    {
        public Blog Blogs { get; set; }
        public Category Categorys { get; set; }
        public int CategoryCount { get; set; }
    }
}
