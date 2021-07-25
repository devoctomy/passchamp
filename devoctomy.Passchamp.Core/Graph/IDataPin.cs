using System;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface IDataPin
    {
        string Name { get; set; }
        object Value { get; set; }
        Type ValueType { get; set; }
        T GetValue<T>();
    }
}
