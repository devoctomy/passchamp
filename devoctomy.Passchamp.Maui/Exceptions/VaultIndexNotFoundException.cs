namespace devoctomy.Passchamp.Maui.Exceptions;

public class VaultIndexNotFoundException : PasschampMauiException
{
    public VaultIndexNotFoundException(string id)
        : base($"VaultIndex with the id '{id}' not found.")
    {
    }
}
