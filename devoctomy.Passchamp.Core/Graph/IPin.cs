using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph;

public interface IPin
{
    string Name { get; set; }
    object ObjectValue { get; }
}
