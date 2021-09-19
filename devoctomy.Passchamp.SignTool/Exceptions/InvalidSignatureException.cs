namespace devoctomy.Passchamp.SignTool.Exceptions
{
    public class InvalidSignatureException : SignToolException
    {
        public InvalidSignatureException() : base("Signature invalid, data may have been tampered with.")
        {}
    }
}
