using System;

namespace devoctomy.Passchamp.Core.Graph;

public interface IDataPinFactory
{
    IPin Create(
        string name,
        object value);
    IPin Create(
        string name,
        object value,
        Type valueType);
}
