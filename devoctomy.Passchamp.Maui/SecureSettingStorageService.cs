using devoctomy.Passchamp.Maui.Exceptions;
using Newtonsoft.Json;
using System.Reflection;

namespace devoctomy.Passchamp.Maui
{
    public class SecureSettingStorageService : ISecureSettingStorageService
    {
        private readonly ISecureStorage _secureStorage;

        public SecureSettingStorageService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public async Task<bool> LoadAsync(
            string id,
            PropertyInfo property,
            object instance)
        {
            var secureSettingsAttribute = (SecureSettingAttribute)property.GetCustomAttributes(
                typeof(SecureSettingAttribute),
                true).FirstOrDefault();
            if (secureSettingsAttribute != null)
            {
                AssureJsonIgnoreAttributeIsPresent(property);

                var key = $"{id}.{secureSettingsAttribute.Group}.{secureSettingsAttribute.Category}.{property.Name}";
                var setting = await _secureStorage.GetAsync(key);
                property.SetValue(instance, setting);
            }

            return false;
        }

        public async Task SaveAsync(
            string id,
            PropertyInfo property,
            object instance)
        {
            var secureSettingsAttribute = (SecureSettingAttribute)property.GetCustomAttributes(
                typeof(SecureSettingAttribute),
                true).FirstOrDefault();
            if (secureSettingsAttribute != null)
            {
                AssureJsonIgnoreAttributeIsPresent(property);

                var key = $"{id}.{secureSettingsAttribute.Group}.{secureSettingsAttribute.Category}.{property.Name}";
                var value = property.GetValue(instance);
                await _secureStorage.SetAsync(key, value.ToString());
            }
        }
        
        private void AssureJsonIgnoreAttributeIsPresent(PropertyInfo property)
        {
            var secureSettingsAttribute = (JsonIgnoreAttribute)property.GetCustomAttributes(
                typeof(JsonIgnoreAttribute),
                true).FirstOrDefault();
            if(secureSettingsAttribute == null)
            {
                throw new MissingJsonIgnoreAttributeException("JsonIgnore Attribute must be used with SecureSetting Attribute.");
            }
        }
    }
}
