using HiClass.API.Helpers;
using HiClass.Application.Common.Exceptions;
using HiClass.Application.Interfaces.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

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
                var contextType = context.Exception.InnerException?.GetType() ?? context.Exception.GetType();
                var exception = context.Exception;

                try
                {
                    if (contextType.GetInterfaces().Any(type =>
                            type == typeof(IUiException) || type == typeof(IDbException)))
                    {
                        return Handle400Exception(context, controllerName, actionName, exception);
                    }

                    if (contextType.GetInterfaces().Any(type => type == typeof(IUiForbiddenException)))
                    {
                        return Handle403Exception(context, controllerName, actionName, exception);
                    }

                    if (contextType.GetInterfaces().Any(type => type == typeof(IServerException)))
                    {
                        return Handle500Exception(context, controllerName, actionName, exception);
                    }

                    return HandleUnknownException(context, controllerName, actionName, exception);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Task.CompletedTask;
                }
            }

            private Task Handle400Exception(ExceptionContext context, string controllerName,
                string actionName, Exception exception)
            {
                GetDeserializedExceptionDto(exception.Message,
                    out var exceptionMessageForUi, out var exceptionMessageForLogging);

                LogException(controllerName, actionName, exception, exceptionMessageForLogging);

                var result = ResponseHelper.GetExceptionObjectResult(exceptionMessageForUi);

                context.HttpContext.Response.StatusCode = 400;
                context.Result = result;
                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }

            private Task Handle403Exception(ExceptionContext context, string controllerName,
                string actionName, Exception exception)
            {
                GetDeserializedExceptionDto(exception.Message,
                    out var exceptionMessageForUi, out var exceptionMessageForLogging);

                LogException(controllerName, actionName, exception, exceptionMessageForLogging);

                var result = ResponseHelper.GetExceptionObjectResult(exceptionMessageForUi);

                context.HttpContext.Response.StatusCode = 403;
                context.Result = result;
                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }

            private Task Handle500Exception(ExceptionContext context, string controllerName,
                string actionName, Exception exception)
            {
                GetDeserializedExceptionDto(exception.Message,
                    out var exceptionMessageForUi, out var exceptionMessageForLogging);

                Log500Exception(controllerName, actionName, exception, exceptionMessageForLogging);

                var result = ResponseHelper.GetExceptionObjectResult(exceptionMessageForUi);
                
                context.HttpContext.Response.StatusCode = 500;
                context.Result = result;
                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }

            private Task HandleUnknownException(ExceptionContext context, string controllerName,
                string actionName, Exception exception)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new StatusCodeResult(500);

                _logger.LogError(exception, "\t\nUnhandled exception!\n");
                Log500Exception(controllerName, actionName, exception, exception.Message);

                context.ExceptionHandled = true;

                return Task.CompletedTask;
            }


            private void LogException(string controllerName,
                string actionName, Exception exception, string exceptionMessageForLogging)
            {
                var logMessage = $"\tController: {controllerName}\n" +
                                 $"\tAction: {actionName}\n" +
                                 $"\tExceptionName: {exception.GetType().Name}\n" +
                                 $"\tMessage: {exceptionMessageForLogging}";

                _logger.LogError("{Message}", logMessage);
            }

            private void Log500Exception(string controllerName, string actionName,
                Exception exception, string exceptionMessageForLogging)
            {
                var logMessage = $"Source: {exception.Source}\n" +
                                 $"\tController: {controllerName}\n" +
                                 $"\tAction: {actionName}\n" +
                                 $"\tExceptionName: {exception.GetType().Name}\n" +
                                 $"\tMessage: {exceptionMessageForLogging}\n" +
                                 $"\tStackTrace: {exception.StackTrace}";

                _logger.LogError("{Message}", logMessage);
            }

            private void GetDeserializedExceptionDto(string exceptionMessage,
                out string exceptionMessageForUi, out string exceptionMessageForLogging)
            {
                try
                {
                    var exceptionDto = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<ExceptionDto>(exceptionMessage);

                    exceptionMessageForUi = exceptionDto!.ExceptionMessageForUi;
                    exceptionMessageForLogging = exceptionDto.ExceptionMessageForLogging;
                }
                catch
                {
                    exceptionMessageForUi = DefaultUiExceptionMessage;
                    exceptionMessageForLogging = exceptionMessage;
                }
            }
        }
    }
}