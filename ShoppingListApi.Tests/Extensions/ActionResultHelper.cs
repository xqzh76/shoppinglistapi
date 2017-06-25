namespace ShoppingListApi.Tests.Extensions
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class ActionResultHelper
    {
        public static T GetOkObject<T>(IActionResult result) where T : class 
        {
            var okObjectResult = result as OkObjectResult;
            if (okObjectResult == null)
            {
                throw new InvalidOperationException($"The result must be of type {nameof(OkObjectResult)}");
            }

            var value = okObjectResult.Value as T;
            if (value == null)
            {
                throw new InvalidOperationException($"The value in the result must be of type {nameof(T)}");
            }

            return value;
        }
    }
}