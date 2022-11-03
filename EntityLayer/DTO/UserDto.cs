using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;

namespace EntityLayer.DTO
{
    public class UserDto : AppUser
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
