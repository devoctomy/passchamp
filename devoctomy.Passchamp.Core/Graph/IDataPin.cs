using System;

namespace devoctomy.Passchamp.Core.Graph
{
    public interface IDataPin<T> : IPin
    {
        T Value { get; set; }
    }
}
