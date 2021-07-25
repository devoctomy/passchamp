namespace devoctomy.Passchamp.Core.Graph
{
    public interface IDataPin
    {
        string Name { get; set; }
        object Value { get; set; }
        T GetValue<T>();
    }
}
