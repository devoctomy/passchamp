namespace devoctomy.Passchamp.Maui.Exceptions
{
    public class ObjectDoesNotImplementIPartiallySecureException : PasschampMauiException
    {
        public ObjectDoesNotImplementIPartiallySecureException(Type type)
            : base($"Object of type {type.Name} does not implement IPartiallySecure.")
        {
        }
    }
}
