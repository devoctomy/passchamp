using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Vault
{
    public class Credential
    {
        public string ID { get; set; }
        public string GlyphKey { get; set; }
        public string GlyphColour { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string CreatedAt { get; set; }
        public string LastUpdatedAt { get; set; }
        public string PasswordLastModifiedAt { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Tags { get; set; }
        public string Notes { get; set; }
        public List<AuditLogEntry> AuditLogEntries { get; set; }
    }
}
