namespace devoctomy.Passchamp.Core.Graph.Presets;

public class NodeConnection(
    string nodeFromKey,
    string pinFromKey,
    string nodeToKey,
    string pinToKey)
{
    public string NodeFromKey => nodeFromKey;
    public string PinFromKey => pinFromKey;
    public string NodeToKey => nodeToKey;
    public string PinToKey => pinToKey;
}
