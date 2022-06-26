namespace devoctomy.Passchamp.Maui
{
    public class SecureSettingsServiceBase : ISecureSettingsService
    {
        public virtual string Group { get; } = "Default";

        private readonly ISecureStorage _secureStorage;

        public SecureSettingsServiceBase(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public async Task Load()
        {
            var type = GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance).ToList();
            foreach(var curProperty in allProperties)
            {
                var secureSettingsAttribute = (SecureSettingAttribute)curProperty.GetCustomAttributes(typeof(SecureSettingAttribute), true).FirstOrDefault();
                if(secureSettingsAttribute != null)
                {
                    var key = $"{Group}.{secureSettingsAttribute.Category}.{curProperty.Name}";
                    var setting = await _secureStorage.GetAsync(key);
                    var propertyType = curProperty.PropertyType;
                    switch(propertyType.Name)
                    {
                        case "Int32":
                            {
                                curProperty.SetValue(this, Int32.Parse(setting));
                                break;
                            }

                        case "Int64":
                            {
                                curProperty.SetValue(this, Int64.Parse(setting));
                                break;
                            }

                        case "Single":
                            {
                                curProperty.SetValue(this, Single.Parse(setting));
                                break;
                            }

                        case "String":
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
                    var key = $"{Group}.{secureSettingsAttribute.Category}.{curProperty.Name}";
                    var value = curProperty.GetValue(this);
                    await _secureStorage.SetAsync(key, value.ToString());
                }
            }
        }
    }
}
