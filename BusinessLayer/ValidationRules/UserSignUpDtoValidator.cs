using BusinessLayer.Abstract;
using EntityLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class UserSignUpDtoValidator : AbstractValidator<UserSignUpDto>
    {
        readonly IBusinessUserService _businessUserService;
        public UserSignUpDtoValidator(IBusinessUserService businessUserService)
        {
            _businessUserService = businessUserService;
        }
        public UserSignUpDtoValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().NotNull().Length(3, 100);
            RuleFor(x => x.UserName).NotEmpty().NotNull().Length(3, 30);
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(3);
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(5).Equal(x => x.ConfirmPassword);
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull();
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.About).NotEmpty().NotNull();
        }
    }
}
