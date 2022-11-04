﻿using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().NotNull().Equal(x=> x.PasswordConfirm);
            RuleFor(x => x.PasswordConfirm).NotEmpty().NotNull();
        }
    }
}