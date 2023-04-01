using Core.Extensions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Extensions
{
    public class ValidationErrorDetail : ErrorDetails
    {
        public IEnumerable<ValidationFailure> ValidationErrors { get; set; }

    }
}
