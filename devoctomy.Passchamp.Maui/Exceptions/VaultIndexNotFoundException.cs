namespace devoctomy.Passchamp.Maui.Exceptions;

public class VaultIndexNotFoundException(string id) : PasschampMauiException($"VaultIndex with the id '{id}' not found.")
{
}
