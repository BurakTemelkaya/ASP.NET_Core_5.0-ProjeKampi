using EntityLayer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class UserValidator:AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x=> x.NameSurname).NotEmpty();
            RuleFor(x=> x.UserName).NotEmpty();
            RuleFor(x=> x.Password).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PasswordAgain).Matches(x => x.Password);

        }
    }
}
