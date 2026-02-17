using System.Net.Mime;
using ELTBackend.Constants;
using ELTBackend.Exceptions;
using ELTBackend.Utilities;

namespace ELTBackend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dictionary<Type, int> _businessExceptionInfo = new Dictionary<Type, int>
        {
            { typeof(InvalidCredentialsException), StatusCodes.Status401Unauthorized },
            { typeof(NotFoundException), StatusCodes.Status404NotFound },
            { typeof(ConflictException), StatusCodes.Status409Conflict },
        };
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(
                    "Business exception occurred. Type={ExceptionType}, Message={Message}, Path={Path}",
                    ex.GetType().Name,
                    ex.Message,
                    context.Request.Path
                );

                context.Response.ContentType = MediaTypeNames.Application.Json;

                if (_businessExceptionInfo.TryGetValue(ex.GetType(), out var statusCode))
                {
                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(
                        ResponseHelper.Failure(errors: new List<string> { ex.Message })
                    );
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(
                        ResponseHelper.Failure(
                            errors: new List<string> { BusinessErrorMessages.InternalServerError }
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled system exception. Path={Path}, Method={Method}",
                    context.Request.Path,
                    context.Request.Method
                );

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsJsonAsync(
                    ResponseHelper.Failure(message: BusinessErrorMessages.InternalServerError)
                );
            }
        }
    }
}
