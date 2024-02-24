using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Vault;

public class VaultSerialiserNode : NodeBase
{
    [NodeInputPin(ValueType = typeof(Core.Vault.Vault), DefaultValue = default(Core.Vault.Vault))]
    public IDataPin<Core.Vault.Vault> Vault
    {
        get
        {
            return GetInput<Core.Vault.Vault>("Vault");
        }
        set
        {
            Input["Vault"] = value;
        }
    }

    [NodeOutputPin(ValueType = typeof(string), DefaultValue = default(string))]
    public IDataPin<string> VaultJson
    {
        get
        {
            return GetOutput<string>("VaultJson");
        }
    }

    protected override Task DoExecuteAsync(
        IGraph graph,
        CancellationToken cancellationToken)
    {
        VaultJson.Value = JsonConvert.SerializeObject(Vault.Value, Formatting.None);
        return Task.CompletedTask;
    }
}
