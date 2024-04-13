using BusinessLayer.Models;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class NewsLetterSendMailsModelValidator : AbstractValidator<NewsLetterSendMailsModel>
    {
        public NewsLetterSendMailsModelValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Mesaj taslağının başlığı boş olamaz");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Mesaj taslağının içeriği boş olamaz");
        }
    }
}
