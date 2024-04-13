using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CoreDemo.Models
{
    public class UserSignUpViewModel : AppUser
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAcceptTheContract { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
