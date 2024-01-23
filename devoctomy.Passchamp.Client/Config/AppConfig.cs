using devoctomy.Passchamp.Core.Data;

namespace devoctomy.Passchamp.Client.Config;

public class AppConfig : IPartiallySecure
{
    public string Id { get; set; }
    public string Theme { get; set; } = "System";
}
