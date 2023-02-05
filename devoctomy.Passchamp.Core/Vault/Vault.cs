using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Vault;

public class Vault
{
    public VaultHeader Header { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }
    public string LastUpdatedAt { get; set; }
    public List<Credential> Credentials { get; set; }
    public List<AuditLogEntry> AuditLogEntries { get; set; }
}
