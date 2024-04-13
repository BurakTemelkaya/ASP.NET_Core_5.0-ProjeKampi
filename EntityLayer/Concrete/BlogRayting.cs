using CoreLayer.Entities;

namespace EntityLayer.Concrete
{
    public class BlogRayting : IEntity
    {
        public int BlogRaytingID { get; set; }
        public int BlogID { get; set; }
        public int BlogTotalScore { get; set; }
        public int BlogRaytingCount { get; set; }
        public double BlogRaytingAverage { get; set; }
    }
}
