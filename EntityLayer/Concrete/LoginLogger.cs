using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete
{
    public class LoginLogger : IEntity
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public DateTime LoginDate { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
