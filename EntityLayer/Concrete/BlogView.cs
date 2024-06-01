using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete;

public class BlogView : IEntity
{
    public int Id { get; set; }
    public string IpAddress { get; set; }
    public int ViewCount { get; set; }
    public Blog Blog { get; set; }
    public int BlogId { get; set; }
    public DateTime ViewingDate { get; set; }
}
