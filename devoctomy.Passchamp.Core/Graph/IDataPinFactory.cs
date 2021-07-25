using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph
{
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
}
