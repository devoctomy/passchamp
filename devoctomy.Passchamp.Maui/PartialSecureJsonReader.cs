using devoctomy.Passchamp.Maui.Exceptions;
using Newtonsoft.Json;

namespace devoctomy.Passchamp.Maui
{
    public class PartialSecureJsonReader : IPartialSecureJsonReader
    {
        private ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureJsonReader(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public async Task<T> LoadAsync<T>(Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            T result;
            using (StreamReader sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                result = serializer.Deserialize<T>(reader);
            }

            await LoadSecureSettingsAsync<T>(result);
            return (T)result;
        }

        private async Task LoadSecureSettingsAsync<T>(T value)
        {
            var partiallySecure = value as IPartiallySecure;
            var type = value.GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance).ToList();
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (partiallySecure == null)
                    {
                        throw new ObjectDoesNotImplementIPartiallySecureException(type);
                    }

                    await _secureSettingStorageService.LoadAsync(
                        partiallySecure.Id,
                        curProperty,
                        value);
                }
            }
        }
    }
}
