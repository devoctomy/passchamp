namespace devoctomy.Passchamp.Maui.Services.Attributes
{
    public class SecureSettingAttribute : Attribute
    {
        public string Group { get; set; }
        public string Category { get; set; }
    }
}
