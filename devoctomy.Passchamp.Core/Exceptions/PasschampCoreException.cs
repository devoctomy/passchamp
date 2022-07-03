using System;

namespace devoctomy.Passchamp.Core.Exceptions
{
    public class PasschampCoreException : Exception
    {
        public PasschampCoreException(string message)
            : base(message)
        {
        }
    }
}
