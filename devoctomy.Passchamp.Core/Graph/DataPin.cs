namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPin
    {
        public object Value { get; set; }

        public DataPin(object value)
        {
            Value = value;
        }

        public T GetValue<T>()
        {
            return (T)Value;
        }
    }
}