namespace devoctomy.Passchamp.Maui.Exceptions;

public class VaultAlreadyIndexedException(string id) : PasschampMauiException($"Vault already index with the id '{id}'.")
{
}
