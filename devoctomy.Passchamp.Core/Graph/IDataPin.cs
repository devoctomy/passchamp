namespace devoctomy.Passchamp.Core.Graph
{
    public interface IDataPin
    {
        object Value { get; set; }
        T GetValue<T>();
    }
}
