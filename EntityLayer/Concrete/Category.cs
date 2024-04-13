using CoreLayer.Entities;
using System.Collections.Generic;

namespace EntityLayer.Concrete
{
    public class Category : IEntity
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public bool CategoryStatus { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
