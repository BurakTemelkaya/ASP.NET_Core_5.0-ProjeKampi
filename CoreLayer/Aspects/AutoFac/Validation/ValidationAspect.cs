using Castle.DynamicProxy;
using CoreLayer.CrossCuttingConcerns.Validation;
using CoreLayer.Utilities.Interceptors;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace CoreLayer.Aspects.AutoFac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private readonly Type _validatorType;
        private readonly IHttpContextAccessor _contextAccessor;

        public ValidationAspect(Type validatorType)
        {
            //defensive coding
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("This is not a validation class");
            }

            _validatorType = validatorType;
            _contextAccessor = (IHttpContextAccessor)Activator.CreateInstance(typeof(HttpContextAccessor));
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t != null && t.GetType() == entityType);
            List<ValidationException> validationExceptions = new();

            foreach (var entity in entities)
            {
                var validationException = ValidationTool.Validate(validator, entity);
                if (validationException != null)
                    validationExceptions.Add(validationException);
            }

            if (validationExceptions.Any())
            {
                List<string> messages = new();
                foreach (var item in validationExceptions)
                {
                    messages.Add(item.Message);
                }

                var messagesJSON = JsonSerializer.Serialize(messages);

                _contextAccessor.HttpContext.Session.SetString("ValidationExceptions", JsonSerializer.Serialize(messagesJSON));

                throw new ValidationException(messagesJSON);
            }               
        }
    }
}
