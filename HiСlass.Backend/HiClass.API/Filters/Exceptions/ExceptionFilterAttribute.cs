using HiClass.API.Helpers;
using HiClass.Application.Common.Exceptions;
using HiClass.Application.Interfaces.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using HiClass.Application.Common.Exceptions.Serialization;

namespace HiClass.API.Filters.Exceptions
{
    public class ExceptionFilterAttribute : TypeFilterAttribute
    {
        public ExceptionFilterAttribute() : base(typeof(ExceptionFilterImplementation))
        {
        }

        private class ExceptionFilterImplementation : IAsyncExceptionFilter
        {
            private const string DefaultUiExceptionMessage = "Something went wrong.";
            private readonly ILogger<ExceptionFilterImplementation> _logger;

            public ExceptionFilterImplementation(ILogger<ExceptionFilterImplementation> logger)
            {
                _logger = logger;
            }

            public Task OnExceptionAsync(ExceptionContext context)
            {
                var controllerName = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
                var actionName = context.RouteData.Values["action"]?.ToString() ?? string.Empty;
                var exception = context.Exception;
                var exceptionType = exception.InnerException?.GetType() ?? exception.GetType();

                try
                {
                    if (exceptionType.GetInterfaces()
                        .Any(type => type == typeof(IUiException) || type == typeof(IDbException)))
                    {
                        return HandleException(context, controllerName, actionName, exception, 400);
                    }

                    if (exceptionType.GetInterfaces().Any(type => type == typeof(IUiForbiddenException)))
                    {
                        return HandleException(context, controllerName, actionName, exception, 403);
                    }

                    if (exceptionType.GetInterfaces().Any(type => type == typeof(IServerException)))
                    {
                        return HandleException(context, controllerName, actionName, exception, 500);
                    }

                    return HandleUnknownException(context, controllerName, actionName, exception);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in exception filter: {Message}", ex.Message);
                    return Task.CompletedTask;
                }
            }

            private Task HandleException(ExceptionContext context, string controllerName, string actionName,
                Exception exception, int statusCode)
            {
                var (exceptionMessageForUi, exceptionMessageForLogging) =
                    GetDeserializedExceptionDto(exception.Message);

                LogException(controllerName, actionName, exception, exceptionMessageForLogging);

                var result =
                    ResponseHelper.GetExceptionObjectResult(GetExceptionDto(exception.GetType().Name,
                        exceptionMessageForUi));
                context.HttpContext.Response.StatusCode = statusCode;
                context.Result = result;
                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }

            private Task HandleUnknownException(ExceptionContext context, string controllerName, string actionName,
                Exception exception)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new StatusCodeResult(500);

                LogException(controllerName, actionName, exception, exception.Message, true);
                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }

            private void LogException(string controllerName, string actionName, Exception exception,
                string exceptionMessageForLogging, bool isUnknownException = false)
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
                
                var logMessage = $"[{timestamp}] Error occurred:\n" +
                                $"Source: {exception.Source}\n" +
                                $"Controller: {controllerName}\n" +
                                $"Action: {actionName}\n" +
                                $"ExceptionType: {exception.GetType().Name}\n" +
                                $"Message: {exceptionMessageForLogging}\n" +
                                (isUnknownException ? $"StackTrace: {exception.StackTrace}\n" : string.Empty) +
                                $"InnerException: {exception.InnerException?.Message ?? "N/A"}";

                _logger.LogError("{Message}", logMessage);
            }

            private static (string exceptionMessageForUi, string exceptionMessageForLogging) GetDeserializedExceptionDto(
                string exceptionMessage)
            {
                try
                {
                    var exceptionDto =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<ExceptionResponseDto>(exceptionMessage);

                    if (exceptionDto == null)
                    {
                        throw new DeserializationException("Error during deserialization of ExceptionResponseDto.");
                    }

                    return (exceptionDto.ExceptionMessageForUi, exceptionDto.ExceptionMessageForLogging);
                }
                catch
                {
                    return (DefaultUiExceptionMessage, exceptionMessage);
                }
            }

            private static ExceptionDto GetExceptionDto(string exceptionTitle, string exceptionMessage)
            {
                return new ExceptionDto
                {
                    ExceptionTitle = exceptionTitle,
                    ExceptionMessage = exceptionMessage,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
}