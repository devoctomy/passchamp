using System;

namespace devoctomy.Passchamp.Core.Graph
{
    public class DataPin : IDataPin
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type ValueType { get; set; }

        public DataPin(
            string name,
            object value,
            Type valueType)
        {
            if (value != null && value.GetType() != valueType)
            {
                throw new ArgumentException("Type of value must match valueType.");
            }

            Name = name;
            Value = value;
            ValueType = valueType;
        }

        public DataPin(
            string name,
            object value)
        {
            if(value == null)
            {
                throw new NullReferenceException("Value cannot be null, use override with valueType instead.");
            }

            Name = name;
            Value = value;
            ValueType = value.GetType();
        }

        public T GetValue<T>()
        {
            return (T)Value;
        }
    }
}