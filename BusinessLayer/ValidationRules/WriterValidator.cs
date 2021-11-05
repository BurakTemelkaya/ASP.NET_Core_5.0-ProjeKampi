using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterValidator:AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.WriterName).NotEmpty().WithMessage("Ad Boş Bırakılamaz.");
            RuleFor(x => x.WriterMail).NotEmpty().WithMessage("Mail Adresi Boş Bırakılamaz.");
            RuleFor(x => x.WriterPassword).NotEmpty().WithMessage("Şifre Boş Bırakılamaz.");
            RuleFor(x => x.WriterAbout).NotEmpty().WithMessage("Hakkınızda kısmı boş Bırakılamaz.");
            RuleFor(x => x.WriterName).MinimumLength(2).WithMessage("Adınız en az 2 karekter olmalıdır.");
            RuleFor(x => x.WriterName).MaximumLength(50).WithMessage("Lütfen en fazla 50 karekterlik bir ad giriniz.");
            RuleFor(x => x.WriterMail).EmailAddress().WithMessage("Lütfen geçerli bir e-mail giriniz.");
            RuleFor(x => x.WriterPassword).MinimumLength(6).WithMessage("Parolanız en az 6 karakter içermelidir.");
            RuleFor(x => x.WriterPassword).Must(IsPasswordValid).WithMessage("Parolanızda en az bir küçük harf bir büyük harf ve rakam içermelidir.");
        }
        private bool IsPasswordValid(string arg)
        {
            try
            {
                Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[0-9])[A-Za-z\d]");
                return regex.IsMatch(arg);
            }
            catch
            {
                return false;
            }
        }
    }
}
