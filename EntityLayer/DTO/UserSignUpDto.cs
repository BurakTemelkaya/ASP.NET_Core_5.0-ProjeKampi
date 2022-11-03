using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EntityLayer.DTO
{
    public class UserSignUpDto : AppUser
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAcceptTheContract { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
