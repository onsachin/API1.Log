using WebApiOne.Api.Operations;

namespace WebApiOne.Api;

public class UseExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public UseExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IOperationHandler<LogEntity> operationHandler)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var entity = new LogEntity
            {
                Path = context.Request.Path.Value,
                StatusCode = context.Response.StatusCode,
                UserId = context.User.Identity.IsAuthenticated ? context.User.Identity?.Name : "User not authenticated",
                Message = exception.Message,
                LongMessage = exception.InnerException?.Message,
                Source = exception.Source,
                CreateDate = DateTime.UtcNow,
            };
            await operationHandler.Insert(entity);
        }
    }
}
