using System.Net;

namespace devoctomy.Passchamp.Core.Cloud
{
    public class CloudStorageProviderResponse
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public CloudStorageProviderResponse(
            bool isSuccessful,
            HttpStatusCode? httpStatusCode)
        {
            IsSuccessful = isSuccessful;
            HttpStatusCode = httpStatusCode;
        }
    }
}
