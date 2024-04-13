using EntityLayer.Concrete;
using FluentValidation;

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
