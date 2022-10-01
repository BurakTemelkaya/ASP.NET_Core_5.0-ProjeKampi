using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser : IdentityUser<int>
    {
        public string NameSurname { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string City { get; set; }
        public virtual ICollection<Message> SenderUserInfo { get; set; }
        public virtual ICollection<Message> ReceiverUserInfo { get; set; }
    }
}
