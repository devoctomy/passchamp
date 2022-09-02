namespace devoctomy.Passchamp.Core.Exceptions;

public class MissingJsonIgnoreAttributeException : PasschampCoreException
{
    public MissingJsonIgnoreAttributeException(string propertyName)
        : base($"JsonIgnore Attribute is missing from property '{propertyName}'.")
    {
    }
}
