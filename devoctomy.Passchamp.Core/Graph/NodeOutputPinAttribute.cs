using System;

namespace devoctomy.Passchamp.Core.Graph
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NodeOutputPinAttribute : Attribute
    {
        public Type ValueType { get; set; }
        public object DefaultValue { get; set; }
    }
}
