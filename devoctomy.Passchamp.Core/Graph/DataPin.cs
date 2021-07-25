using System;

namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPin<T> : IDataPin<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public object ObjectValue => Value;

        public DataPin(
            string name,
            T value)
        {
            Name = name;
            Value = value;
        }
    }
}