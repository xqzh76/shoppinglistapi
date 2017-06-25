namespace ShoppingListApi.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Serilog;

    public sealed class RequestLoggingMiddleware
    {
        private readonly ILogger logger = Log.ForContext<RequestLoggingMiddleware>();

        private readonly RequestDelegate next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            try
            {
                this.logger.Information($"Before {httpContext.Request.Method} {httpContext.Request.Path}");
                await this.next(httpContext);
                this.logger.Information($"After {httpContext.Request.Method} {httpContext.Request.Path} responded {httpContext.Response?.StatusCode}");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"Uncaught exception in {httpContext.Request.Method} {httpContext.Request.Path}");
                httpContext.Response.StatusCode = 500;
            }
        }
    }
}