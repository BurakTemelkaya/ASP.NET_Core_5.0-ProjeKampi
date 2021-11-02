using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Blog ismi boş geçilemez.");
            RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog içeriği boş geçilemez.");
            RuleFor(x => x.BlogImage).NotEmpty().WithMessage("Blog görseli boş geçilemez.");
            RuleFor(x => x.BlogThumbnailImage).NotEmpty().WithMessage("Blog küçük görseli boş geçilemez.");
            RuleFor(x => x.BlogTitle).MaximumLength(150).WithMessage("Blog başlığı en fazla 150 karekter olabilir.");
            RuleFor(x => x.BlogTitle).MinimumLength(5).WithMessage("Blog başlığı en az 5 karekter olabilir.");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Kategori Boş Bırakılamaz");
        }
    }
}
