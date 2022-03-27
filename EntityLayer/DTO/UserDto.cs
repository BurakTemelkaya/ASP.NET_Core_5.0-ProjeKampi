using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class UserDto : AppUser
    {
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}
