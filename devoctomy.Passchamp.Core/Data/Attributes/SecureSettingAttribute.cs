using System;

namespace devoctomy.Passchamp.Core.Data.Attributes
{
    public class SecureSettingAttribute : Attribute
    {
        public string Group { get; set; }
        public string Category { get; set; }
    }
}
