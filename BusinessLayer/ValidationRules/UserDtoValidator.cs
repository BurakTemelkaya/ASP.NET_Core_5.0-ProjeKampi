using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.DTO;

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
