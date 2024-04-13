using Core.Extensions;
using FluentValidation.Results;
using System.Collections.Generic;

namespace CoreLayer.Extensions
{
    public class ValidationErrorDetail : ErrorDetails
    {
        public IEnumerable<ValidationFailure> ValidationErrors { get; set; }

    }
}
