namespace devoctomy.Passchamp.Core.Graph;

public interface IPin
{
    string Name { get; set; }
    object ObjectValue { get; }
}
