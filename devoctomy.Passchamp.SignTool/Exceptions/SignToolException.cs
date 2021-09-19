using System;

namespace devoctomy.Passchamp.SignTool.Exceptions
{
    public class SignToolException : Exception
    {
        public SignToolException(string message) : base(message)
        {}

        public SignToolException(
            string message,
            Exception innerException) : base(message, innerException)
        { }
    }
}
