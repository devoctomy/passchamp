using Newtonsoft.Json;
using System;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class TestPartialSecureConfigFile : PartialSecureConfigFileBase
    {
        public string TestSetting1 { get; set; }
        public int TestSetting2 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        [JsonIgnore]
        public string TestSetting3 { get; set; }

        public TestPartialSecureConfigFile(ISecureSettingStorageService secureSettingStorageService)
            : base(secureSettingStorageService)
        {
            Id = "TestPartialSecureConfigFile";
        }
    }
}
