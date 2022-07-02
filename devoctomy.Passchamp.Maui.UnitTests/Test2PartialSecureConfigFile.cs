﻿using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class Test2PartialSecureConfigFile : IPartiallySecure
    {
        public string Id { get; set; } = "TestPartialSecureConfigFile";
        public string TestSetting1 { get; set; }
        public int TestSetting2 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        public string TestSetting3 { get; set; }

        [SecureSetting(Group = "Group", Category = "Category")]
        [JsonIgnore]
        public string TestSetting4 { get; set; }
    }
}
