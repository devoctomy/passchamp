using System.Text.Json.Serialization;

namespace devoctomy.Passchamp.Core.Cloud;

public class CloudStorageProviderConfigRef
{
    public string Id { get; set; }
    public string ProviderServiceTypeId { get; set; }
    [JsonIgnore]
    public string DisplayName { get; private set; }

    public void SetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    public override string ToString()
    {
        return DisplayName;
    }
}
