namespace devoctomy.Passchamp.Maui.Exceptions
{
    public class MissingJsonIgnoreAttributeException : PasschampMauiException
    {
        public MissingJsonIgnoreAttributeException(string propertyName)
            : base($"JsonIgnore Attribute is missing from property '{propertyName}'.")
        {
        }
    }
}
