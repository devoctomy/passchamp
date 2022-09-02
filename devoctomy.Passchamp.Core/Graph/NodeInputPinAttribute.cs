using System;

namespace devoctomy.Passchamp.Core.Graph;

[AttributeUsage(AttributeTargets.Property)]
public class NodeInputPinAttribute : Attribute
{
    public Type ValueType { get; set; }
    public object DefaultValue { get; set; }
}
