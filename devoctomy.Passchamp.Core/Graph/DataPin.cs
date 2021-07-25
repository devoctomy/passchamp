namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPin : IDataPin
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public DataPin(
            string name,
            object value)
        {
            Name = name;
            Value = value;
        }

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