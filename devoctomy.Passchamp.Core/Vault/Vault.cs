using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Vault;

public class Vault
{
    public VaultHeader Header { get; set; }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"); // Explicit date time format needs to be used
    public string LastUpdatedAt { get; set; }
    public List<Credential> Credentials { get; set; } = new List<Credential>();
    public List<AuditLogEntry> AuditLogEntries { get; set; } = new List<AuditLogEntry>();
}
