namespace ShoppingListApi.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Serilog.Context;

    public sealed class CorrelationIdLoggingMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";
        private readonly RequestDelegate next;

        public CorrelationIdLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            string corrleationId = null;

            // Try to get the correlationId from header; if it doesn't exist, create a new one
            StringValues correlationIdValues;
            if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out correlationIdValues))
            {
                corrleationId = correlationIdValues.FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(corrleationId))
            {
                corrleationId = Guid.NewGuid().ToString();
            }

            using (LogContext.PushProperty("CorrelationId", corrleationId))
            {
                return this.next(context);
            }
        }
    }
}