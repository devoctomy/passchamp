using devoctomy.Passchamp.Core.Graph;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Test
{
    public class TestNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(string), DefaultValue = "Hello World")]
        public IDataPin<string> InputTest
        {
            get
            {
                return GetInput<string>("InputTest");
            }
            set
            {
                Input["InputTest"] = value;
            }
        }

        [NodeOutputPin(ValueType = typeof(string), DefaultValue = "Hello World")]
        public IDataPin<string> OutputTest
        {
            get
            {
                return GetOutput<string>("OutputTest");
            }
            set
            {
                Output["OutputTest"] = value;
            }
        }
    }
}
