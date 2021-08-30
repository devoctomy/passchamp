using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Vault
{
    public class Vault
    {
        public VaultHeader Header { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string LastUpdatedAt { get; set; }
        public List<Credential> Credentials { get; set; }
    }
}
