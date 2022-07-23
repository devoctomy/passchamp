using System;

namespace devoctomy.Passchamp.Core.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SecureSettingAttribute : Attribute
    {
        public string Group { get; set; }
        public string Category { get; set; }
    }
}
