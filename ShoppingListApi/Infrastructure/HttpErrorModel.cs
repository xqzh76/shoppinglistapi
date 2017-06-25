namespace ShoppingListApi.Infrastructure
{
    public sealed class HttpErrorModel
    {
        public HttpErrorModel(string error, string errorDescription, string errorUri)
        {
            this.Error = error;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
        }

        public string Error { get; }

        public string ErrorDescription { get; }

        public string ErrorUri { get; }

        public override string ToString()
        {
            var errorString = $"{this.Error}: {this.ErrorDescription}";

            if (!string.IsNullOrEmpty(this.ErrorUri))
            {
                return errorString + $" For more inforation on this error, see {this.ErrorUri}";
            }

            return errorString;
        }
    }
}