using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using CoreLayer.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        private LoggerServiceBase _databaseLoggerServiceBase;

        private LoggerServiceBase _fileLoggerServiceBase;

        private Exception _lastException;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;

            _databaseLoggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(typeof(DatabaseLogger));

            _fileLoggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(typeof(FileLogger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                HandleException(exception);
                await _next(httpContext);
            }
        }

        private void HandleException(Exception exception)
        {
            if (exception == _lastException)
            {
                return;
            }

            _lastException = exception;
            
            IEnumerable<ValidationFailure> errors;

            if (exception.GetType() == typeof(ValidationException))
            {
                string message = exception.Message;
                errors = ((ValidationException)exception).Errors;

                var exceptionModel = new ValidationErrorDetail
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = message,
                    ValidationErrors = errors
                };

                _databaseLoggerServiceBase.Error(exceptionModel);

                _fileLoggerServiceBase.Error(exceptionModel);
            }
            else
            {
                _databaseLoggerServiceBase.Error(exception);
                _fileLoggerServiceBase.Error(exception);
            }
        }
    }
}
