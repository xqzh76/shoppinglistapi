namespace ShoppingListApi.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public sealed class HttpError : List<HttpErrorModel>
    {
        public HttpError(string errorCode, string errorDescription, string errorUri)
        {
            this.Add(new HttpErrorModel(errorCode, errorDescription, errorUri));
        }

        public HttpError(ModelStateDictionary modelStateDictionary)
        {
            foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in modelStateDictionary)
            {
                var errorKey = $" invalid {keyValuePair.Key}";
                ModelErrorCollection errors = keyValuePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorDescription = string.Join(";", errors.Select(e => e.ErrorMessage));
                    this.Add(new HttpErrorModel(errorKey, errorDescription, string.Empty));
                }
            }
        }

        public override string ToString()
        {
            return string.Join(";", this);
        }
    }
} 
