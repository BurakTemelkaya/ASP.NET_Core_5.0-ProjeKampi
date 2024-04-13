using CoreLayer.Entities;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;

namespace EntityLayer.DTO
{
    public class UserSignUpDto : AppUser, IDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAcceptTheContract { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
