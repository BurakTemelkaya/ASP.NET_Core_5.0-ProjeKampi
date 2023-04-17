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
        public UserSignUpDtoValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Ad soyad boş olamaz.").Length(3, 100).WithMessage("Ad soyad en az 3 en fazla 100 karekter olabilir");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz").Length(3, 30).WithMessage("Kullanıcı adı en az 3 en fazla 30 karekter olabilir.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail adresi boş olamaz.").EmailAddress().WithMessage("Mail adresi geçerli olmak zorundadır.").Length(5, 250).WithMessage("Mail adresi en az 5 en fazla 250 karekter olabilir");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş olamaz.").Length(5, 50).WithMessage("Parola en az 5 en fazla 50 karekter olabilir").Equal(x => x.ConfirmPassword).WithMessage("Parola ile parola tekrarı aynı olmak zorunda");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Parola tekrarı boş geçilemez");
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.About).NotEmpty().WithMessage("Hakkında kısmı boş geçilemez");
        }
    }
}
