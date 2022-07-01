using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class Test2PartialSecureConfigFile : PartialSecureConfigFileBase
    {
        public string TestSetting1 { get; set; }
        public int TestSetting2 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        [JsonIgnore]
        public string TestSetting3 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        public string TestSetting4 { get; set; }

        public Test2PartialSecureConfigFile(ISecureSettingStorageService secureSettingStorageService)
            : base(secureSettingStorageService)
        {
            Id = "Test2PartialSecureConfigFile";
        }
    }
}
