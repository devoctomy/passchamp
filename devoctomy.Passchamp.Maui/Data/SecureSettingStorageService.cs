using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Core.Data.Attributes;
using System.Reflection;

namespace devoctomy.Passchamp.Maui.Data
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

            var key = $"{id}.{secureSettingsAttribute.Group}.{secureSettingsAttribute.Category}.{property.Name}";
            var value = property.GetValue(instance);
            await _secureStorage.SetAsync(key, value.ToString());
        }
    }
}
