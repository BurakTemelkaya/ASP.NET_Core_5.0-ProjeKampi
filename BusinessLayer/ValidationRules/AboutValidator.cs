using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(x => x.AboutDetails1).NotEmpty().WithMessage("Hakkında detay 1 kısmı boş geçilemez.");
            RuleFor(x => x.AboutDetails2).NotEmpty().WithMessage("Hakkında detay 2 kısmı boş geçilemez.");
        }
    }
}
