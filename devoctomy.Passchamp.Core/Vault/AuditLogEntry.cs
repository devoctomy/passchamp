using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Vault
{
    public class AuditLogEntry
    {
        public string DateTime { get; set; }
        public string TypeOfEntry { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
