using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class NewsLetterValidator : AbstractValidator<NewsLetter>
    {
        public NewsLetterValidator()
        {
            RuleFor(x => x.Mail).NotEmpty().WithMessage("Abone bülteni kaydının mail adresi boş olamaz").EmailAddress().WithMessage("Abone bülteni kaydının mail adresi geçerli olmalıdır.");
        }
    }
}
