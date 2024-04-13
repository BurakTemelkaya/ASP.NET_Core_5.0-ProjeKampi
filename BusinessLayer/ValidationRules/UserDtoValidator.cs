using EntityLayer.DTO;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().NotNull().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.UserName).NotEmpty().NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(3);
        }
    }
}
