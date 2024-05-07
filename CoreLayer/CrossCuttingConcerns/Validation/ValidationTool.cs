using FluentValidation;

namespace CoreLayer.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static ValidationException Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                return new ValidationException(result.Errors);
            }

            return null;
        }
    }
}
