namespace ShoppingListApi.Infrastructure
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Serilog;

    public sealed class ModelValidationFilter : ActionFilterAttribute
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ModelValidationFilter>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var httpError = new HttpError(context.ModelState);

                Log.Information(string.Join("; ", httpError));

                context.Result = new BadRequestObjectResult(httpError);
            }
        }
    }
}