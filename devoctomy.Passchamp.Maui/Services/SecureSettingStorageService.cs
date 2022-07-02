using devoctomy.Passchamp.Maui.Exceptions;
using devoctomy.Passchamp.Maui.Services.Attributes;
using Newtonsoft.Json;
using System.Reflection;

namespace devoctomy.Passchamp.Maui.Services
{
    public class SecureSettingStorageService : ISecureSettingStorageService
    {
        private readonly ISecureStorage _secureStorage;

        public SecureSettingStorageService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public bool IsApplicable(PropertyInfo property)
        {
            var secureSettingsAttribute = (SecureSettingAttribute)property.GetCustomAttributes(
                typeof(SecureSettingAttribute),
                true).FirstOrDefault();
            return secureSettingsAttribute != null;
        }

        public async Task<bool> LoadAsync(
            string id,
            PropertyInfo property,
            object instance)
        {
            var secureSettingsAttribute = (SecureSettingAttribute)property.GetCustomAttributes(
                typeof(SecureSettingAttribute),
                true).Single();
            AssureJsonIgnoreAttributeIsPresent(property);

            var key = $"{id}.{secureSettingsAttribute.Group}.{secureSettingsAttribute.Category}.{property.Name}";
            var setting = await _secureStorage.GetAsync(key);
            property.SetValue(instance, setting);

            return false;
        }

        public async Task SaveAsync(
            string id,
            PropertyInfo property,
            object instance)
        {
            var secureSettingsAttribute = (SecureSettingAttribute)property.GetCustomAttributes(
                typeof(SecureSettingAttribute),
                true).Single();
            AssureJsonIgnoreAttributeIsPresent(property);

            var key = $"{id}.{secureSettingsAttribute.Group}.{secureSettingsAttribute.Category}.{property.Name}";
            var value = property.GetValue(instance);
            await _secureStorage.SetAsync(key, value.ToString());
        }

        private void AssureJsonIgnoreAttributeIsPresent(PropertyInfo property)
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
