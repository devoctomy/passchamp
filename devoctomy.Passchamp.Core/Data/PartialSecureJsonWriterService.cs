using devoctomy.Passchamp.Core.Exceptions;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public class PartialSecureJsonWriterService : IPartialSecureJsonWriterService
    {
        private readonly ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureJsonWriterService(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public void RemoveAll(object value)
        {
            var type = value.GetType();
            var allProperties = type.GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance);
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (value is not IPartiallySecure partiallySecure)
                    {
                        throw new ObjectDoesNotImplementIPartiallySecureException(type);
                    }

                    AssureJsonIgnoreAttributeIsPresent(curProperty);

                    _secureSettingStorageService.Remove(
                        partiallySecure.Id,
                        curProperty);
                }
            }
        }

        public async Task SaveAsync(
            object value,
            Stream stream)
        {
            await SaveSecureSettingsAsync(value);

            var serializer = new JsonSerializer();
            using var sw = new StreamWriter(stream, leaveOpen: true);
            using var writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, value);
        }

        private async Task SaveSecureSettingsAsync(object value)
        {
            var type = value.GetType();
            var allProperties = type.GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance);
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (value is not IPartiallySecure partiallySecure)
                    {
                        throw new ObjectDoesNotImplementIPartiallySecureException(type);
                    }

                    AssureJsonIgnoreAttributeIsPresent(curProperty);

                    await _secureSettingStorageService.SaveAsync(
                        partiallySecure.Id,
                        curProperty,
                        value);
                }
            }
        }

        private static void AssureJsonIgnoreAttributeIsPresent(PropertyInfo property)
        {
            var secureSettingsAttribute = (JsonIgnoreAttribute)property.GetCustomAttributes(
                typeof(JsonIgnoreAttribute),
                true).FirstOrDefault();
            if (secureSettingsAttribute == null)
            {
                throw new MissingJsonIgnoreAttributeException("JsonIgnore Attribute must be used with SecureSetting Attribute.");
            }
        }
    }
}
