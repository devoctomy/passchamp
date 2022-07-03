using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cloud
{
    public interface ICloudStorageProviderConfig
    {
        public string ProviderTypeId { get; }
    }
}
