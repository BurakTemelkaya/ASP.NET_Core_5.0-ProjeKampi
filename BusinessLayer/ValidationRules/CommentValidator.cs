using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.CommentUserName).NotEmpty().WithMessage("Ad Soyad adı boş olamaz").MinimumLength(3).WithMessage("Ad Soyad adı en az 3 karekter olabilir.").MaximumLength(30).WithMessage("Ad Soyad en az fazla 30 karekter olabilir.");

            RuleFor(x => x.CommentTitle).NotEmpty().WithMessage("Başlık boş olamaz").MinimumLength(5).WithMessage("Başlık en az 3 karekter olabilir.").MaximumLength(50).WithMessage("Başlık en az fazla 50 karekter olabilir.");

            RuleFor(x => x.CommentContent).NotEmpty().WithMessage("İçerik boş olamaz").MinimumLength(10).WithMessage("İçerik en az 10 karekter olabilir.").MaximumLength(500).WithMessage("İçerik en fazla 500 karekter olabilir");

            RuleFor(x => x.BlogScore).LessThan(6).GreaterThan(0);

            RuleFor(x => x.BlogID).NotEmpty().NotNull();
        }
    }
}
