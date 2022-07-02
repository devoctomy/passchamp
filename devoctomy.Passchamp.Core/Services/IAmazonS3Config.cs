namespace devoctomy.Passchamp.Core.Services
{
    public interface IAmazonS3Config 
    {
        public string AccessId { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string Bucket { get; set; }
        public string Path { get; set; }
    }
}
