using System;

namespace CoreDemo.Areas.Admin.Models
{
    public class BannedUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BanMessage { get; set; }
        public DateTime BanExpirationTime { get; set; }
    }
}
