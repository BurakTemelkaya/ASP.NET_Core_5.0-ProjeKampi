using System.ComponentModel.DataAnnotations;

namespace CoreDemo.Models
{
    public class UserSignUpViewModel
    {
        [Display(Name ="Ad Soyad")]
        [Required(ErrorMessage ="Lütfen ad ve soyad giriniz.")]
        public string NameSurname { get; set; }
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Lütfen şifre giriniz.")]
        public string Password { get; set; }
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password",ErrorMessage ="Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Mail Adresi")]
        [Required(ErrorMessage = "Lütfen mail adresinizi giriniz.")]
        public string Mail { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz.")]
        public string UserName { get; set; }
        public bool IsAcceptTheContract { get; set; }
    }
}
