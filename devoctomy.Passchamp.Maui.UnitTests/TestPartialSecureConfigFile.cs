using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class TestPartialSecureConfigFile : IPartiallySecure
    {
        public string Id { get; set; } = "TestPartialSecureConfigFile";
        public string TestSetting1 { get; set; }
        public int TestSetting2 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        [JsonIgnore]
        public string TestSetting3 { get; set; }
    }
}
