using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Vault;

public class VaultParserNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(string), DefaultValue = default(string))]
    public IDataPin<string> VaultJson
    {
        get
        {
            return GetInput<string>("VaultJson");
        }
        set
        {
            Input["VaultJson"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(Core.Vault.Vault), DefaultValue = default(Core.Vault.Vault))]
    public IDataPin<Core.Vault.Vault> Vault
    {
        get
        {
            return GetOutput<Core.Vault.Vault>("Vault");
        }
    }


    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        Vault.Value = JsonConvert.DeserializeObject<Core.Vault.Vault>(VaultJson.Value);
        return Task.CompletedTask;
    }
}
