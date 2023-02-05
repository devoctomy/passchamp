using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Data.Attributes;
using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.UnitTests.Data;

public class TestPartialSecureConfigFile : IPartiallySecure, ICloudStorageProviderConfig
{
    public string Id { get; set; }
    public string ProviderTypeId => "Uknown";
    public string DisplayName { get; set; }
    public string TestSetting1 { get; set; }
    public int TestSetting2 { get; set; }

    [SecureSetting(Group = "Group", Category = "Category")]
    [JsonIgnore]
    public string TestSetting3 { get; set; }
}
