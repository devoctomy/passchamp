using System;

namespace devoctomy.Passchamp.Maui
{
    public class SecureSettingAttribute : Attribute
    {
        public string Key { get; set; }
    }
}
