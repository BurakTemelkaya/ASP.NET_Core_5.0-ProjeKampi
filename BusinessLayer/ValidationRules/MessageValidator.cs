using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Details).NotEmpty().WithMessage("Mesaj detayı boş olamaz");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Mesaj başlığı boş olamaz");
        }
    }
}
