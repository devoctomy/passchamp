using Microsoft.Maui.Storage;

namespace devoctomy.Passchamp.Maui.UnitTests
{
    public class TestSecureSettingsService : SecureSettingsServiceBase
    {
        public override string Group { get => "TestSecureSettingsService"; }

        [SecureSetting(Category = "Test")]
        public string TestSetting1 { get; set; }

        [SecureSetting(Category = "Test")]
        public int TestSetting2 { get; set; }

        [SecureSetting(Category = "Test")]
        public float TestSetting3 { get; set; }

        public TestSecureSettingsService(ISecureStorage secureStorage)
            : base(secureStorage)
        {
        }
    }
}
