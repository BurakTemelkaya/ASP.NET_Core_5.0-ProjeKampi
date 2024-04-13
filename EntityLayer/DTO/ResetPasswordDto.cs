using CoreLayer.Entities;

namespace EntityLayer.DTO
{
    public class ResetPasswordDto : IDto
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
