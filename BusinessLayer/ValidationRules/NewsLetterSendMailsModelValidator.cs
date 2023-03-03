using BusinessLayer.Models;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
