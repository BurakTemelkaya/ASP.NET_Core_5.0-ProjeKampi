using EntityLayer.Concrete;

namespace EntityLayer.DTO
{
    public class BlogCategoryandCommentCountandWriterDto : Blog
    {
        public string CategoryName { get; set; }

        public bool CategoryStatus { get; set; }

        public bool CommentStatus { get; set; } = false;

        public int CommentCount { get; set; } = 0;

        public double CommentScore { get; set; } = 0;

        public int WriterId { get; set; }

        public string WriterNameSurName { get; set; }

        public string WriterUserName { get; set; }
        public int BlogViewCount { get; set; }
    }
}
