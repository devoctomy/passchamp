using System.Net;

namespace devoctomy.Passchamp.Core.Cloud;

public class CloudStorageProviderObjectResponse<T>
{
    public bool IsSuccessful { get; set; }
    public HttpStatusCode? HttpStatusCode { get; set; }
    public T Value { get; set; }

    public CloudStorageProviderObjectResponse(
        bool isSuccessful,
        HttpStatusCode? httpStatusCode,
        T value)
    {
        IsSuccessful = isSuccessful;
        HttpStatusCode = httpStatusCode;
        Value = value;
    }
}
