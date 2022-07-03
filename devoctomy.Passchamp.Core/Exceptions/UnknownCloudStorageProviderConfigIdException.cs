namespace devoctomy.Passchamp.Core.Exceptions
{
    public class UnknownCloudStorageProviderConfigIdException : PasschampCoreException
    {
        public UnknownCloudStorageProviderConfigIdException(string id)
            : base($"Unknown cloud storage provider configuration of '{id}'.")
        {
        }
    }
}
