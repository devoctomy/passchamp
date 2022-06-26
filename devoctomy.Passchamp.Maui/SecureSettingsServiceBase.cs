namespace devoctomy.Passchamp.Maui
{
    public class SecureSettingsServiceBase : ISecureSettingsService
    {
        public async Task Load()
        {
            var type = GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            foreach(var curProperty in allProperties)
            {
                var secureSettingsAttribute = (SecureSettingAttribute)curProperty.GetCustomAttributes(typeof(SecureSettingAttribute), true).FirstOrDefault();
                if(secureSettingsAttribute != null)
                {
                    var setting = await SecureStorage.Default.GetAsync(secureSettingsAttribute.Key);
                    var propertyType = curProperty.PropertyType;
                    switch(propertyType.Name)
                    {
                        case "System.Int32":
                            {
                                curProperty.SetValue(this, Int32.Parse(setting));
                                break;
                            }

                        case "System.String":
                            {
                                curProperty.SetValue(this, setting);
                                break;
                            }
                    }
                }
            }
        }

        public async Task Save()
        {
            var type = GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            foreach (var curProperty in allProperties)
            {
                var secureSettingsAttribute = (SecureSettingAttribute)curProperty.GetCustomAttributes(typeof(SecureSettingAttribute), true).FirstOrDefault();
                if (secureSettingsAttribute != null)
                {
                    var value = curProperty.GetValue(this);
                    await SecureStorage.Default.SetAsync(secureSettingsAttribute.Key, value.ToString());
                }
            }
        }
    }
}
