using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui
{
    public class PartialSecureConfigFileBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        private ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureConfigFileBase(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public static async Task<T> LoadAsync<T>(
            IServiceProvider serviceProvider,
            Stream stream) where T : PartialSecureConfigFileBase
        {
            JsonSerializer serializer = new JsonSerializer();
            PartialSecureConfigFileBase result;
            using (StreamReader sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                result = serializer.Deserialize<T>(reader);
            }

            result._secureSettingStorageService = (ISecureSettingStorageService)serviceProvider.GetService(typeof(ISecureSettingStorageService));
            await result.LoadSecureSettingsAsync();
            return (T)result;
        }

        public async Task SaveAsync(Stream stream)
        {
            await SaveSecureSettingsAsync();

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(stream, leaveOpen: true))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, this);
            }
        }

        private async Task LoadSecureSettingsAsync()
        {
            var type = GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance).ToList();
            foreach (var curProperty in allProperties)
            {
                await _secureSettingStorageService.LoadAsync(Id, curProperty, this);
            }
        }

        private async Task SaveSecureSettingsAsync()
        {
            var type = GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            foreach (var curProperty in allProperties)
            {
                await _secureSettingStorageService.SaveAsync(Id, curProperty, this);
            }
        }
    }
}
