using Microsoft.Maui.Storage;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class TestSecureSettingsService : SecureSettingsServiceBase
    {
        [SecureSetting(Key = "Test.TestSetting1")]
        public string TestSetting1 { get; set; }

        [SecureSetting(Key = "Test.TestSetting2")]
        public int TestSetting2 { get; set; }

        [SecureSetting(Key = "Test.TestSetting3")]
        public float TestSetting3 { get; set; }

        public TestSecureSettingsService(ISecureStorage secureStorage)
            : base(secureStorage)
        {
        }
    }
}
