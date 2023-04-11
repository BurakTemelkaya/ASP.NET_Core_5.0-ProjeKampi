using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using CoreLayer.Extensions;
using CoreLayer.Utilities.Messages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        private LoggerServiceBase _databaseLoggerServiceBase;

        private LoggerServiceBase _fileLoggerServiceBase;

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
            catch (Exception e)
            { 
                await HandleExceptionAsync(httpContext, e);              
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";

            IEnumerable<ValidationFailure> errors;

            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var exceptionModel = new ValidationErrorDetail
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = message,
                    ValidationErrors = errors
                };

                _databaseLoggerServiceBase.Error(exceptionModel);

                _fileLoggerServiceBase.Error(exceptionModel);

                return httpContext.Response.WriteAsync(exceptionModel.ToString());
            }

            _databaseLoggerServiceBase.Error(e);
            _fileLoggerServiceBase.Error(e);

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
