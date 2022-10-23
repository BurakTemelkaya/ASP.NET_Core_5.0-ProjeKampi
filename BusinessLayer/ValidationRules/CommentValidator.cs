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
            RuleFor(x => x.CommentUserName).NotEmpty().NotNull().MinimumLength(3);
            RuleFor(x => x.CommentTitle).NotEmpty().NotNull().MinimumLength(5);
            RuleFor(x => x.CommentContent).NotEmpty().NotNull().MinimumLength(10);
        }
    }
}
