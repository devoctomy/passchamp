using System.Net;

namespace devoctomy.Passchamp.Core.Services
{
    public class CloudProviderObjectResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }
        public T Value { get; set; }

        public CloudProviderObjectResponse(
            bool isSuccessful,
            HttpStatusCode? httpStatusCode,
            T value)
        {
            IsSuccessful = isSuccessful;
            HttpStatusCode = httpStatusCode;
            Value = value;
        }
    }
}
