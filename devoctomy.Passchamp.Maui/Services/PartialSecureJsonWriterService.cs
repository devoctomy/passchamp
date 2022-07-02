using devoctomy.Passchamp.Maui.Exceptions;
using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui.Services
{
    public class PartialSecureJsonWriterService : IPartialSecureJsonWriterService
    {
        private ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureJsonWriterService(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public async Task SaveAsync(
            object value,
            Stream stream)
        {
            await SaveSecureSettingsAsync(value);

            JsonSerializer serializer = new JsonSerializer();
            using StreamWriter sw = new StreamWriter(stream, leaveOpen: true);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, value);
        }

        private async Task SaveSecureSettingsAsync(object value)
        {
            var partiallySecure = value as IPartiallySecure;
            var type = value.GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (partiallySecure == null)
                    {
                        throw new ObjectDoesNotImplementIPartiallySecureException(type);
                    }

                    await _secureSettingStorageService.SaveAsync(
                        partiallySecure.Id,
                        curProperty,
                        value);
                }
            }
        }
    }
}
