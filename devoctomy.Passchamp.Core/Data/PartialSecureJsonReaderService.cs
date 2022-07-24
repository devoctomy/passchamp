using devoctomy.Passchamp.Core.Exceptions;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public class PartialSecureJsonReaderService : IPartialSecureJsonReaderService
    {
        private readonly ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureJsonReaderService(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public async Task<T> LoadAsync<T>(Stream stream)
        {
            var serializer = new JsonSerializer();
            T result;
            using (var sr = new StreamReader(stream))
            using (var reader = new JsonTextReader(sr))
            {
                result = serializer.Deserialize<T>(reader);
            }

            await LoadSecureSettingsAsync(result);
            return result;
        }

        private async Task LoadSecureSettingsAsync<T>(T value)
        {
            var type = value.GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance).ToList();
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (value is not IPartiallySecure partiallySecure)
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
