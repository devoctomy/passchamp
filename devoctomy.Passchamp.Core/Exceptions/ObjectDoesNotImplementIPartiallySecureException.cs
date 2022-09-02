using System;

namespace devoctomy.Passchamp.Core.Exceptions;

public class ObjectDoesNotImplementIPartiallySecureException : PasschampCoreException
{
    public ObjectDoesNotImplementIPartiallySecureException(Type type)
        : base($"Object of type {type.Name} does not implement IPartiallySecure.")
    {
    }
}
