using BusinessLayer.Models;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class GetBlogModelValidator : AbstractValidator<GetBlogModel>
    {
        public GetBlogModelValidator()
        {
            RuleFor(x=> x.Take).GreaterThan(0);
            RuleFor(x=> x.Page).GreaterThan(0);
            RuleFor(x=> x.Id).GreaterThan(-1);
        }
    }
}
