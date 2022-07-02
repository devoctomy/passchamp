using System.Net;

namespace devoctomy.Passchamp.Core.Services
{
    public class CloudProviderResponse
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public CloudProviderResponse(
            bool isSuccessful,
            HttpStatusCode? httpStatusCode)
        {
            IsSuccessful = isSuccessful;
            HttpStatusCode = httpStatusCode;
        }
    }
}
