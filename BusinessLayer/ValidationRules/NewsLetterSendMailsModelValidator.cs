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
            RuleFor(x => x.Subject).NotEmpty().NotNull();
            RuleFor(x => x.Content).NotEmpty().NotNull();
        }
    }
}
