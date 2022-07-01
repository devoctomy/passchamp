namespace devoctomy.Passchamp.Maui
{
    public class SecureSettingAttribute : Attribute
    {
        public string Group { get; set; }
        public string Category { get; set; }
    }
}
