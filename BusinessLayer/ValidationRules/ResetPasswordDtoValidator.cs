using EntityLayer.DTO;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş olamaz");
            RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage("Parola tekrarı boş olamaz");
            RuleFor(x => x.Password).Equal(x => x.PasswordConfirm).WithMessage("Parola ile parola tekrarı aynı olmalıdır.");
        }
    }
}
