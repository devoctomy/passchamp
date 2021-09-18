using devoctomy.Passchamp.Core.Graph;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Test
{
    public class TestNode : NodeBase
    {
        public new bool Executed { get; private set; }

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

        public TestNode()
        {
        }

        public TestNode(string inputTestValue)
        {
            InputTest.Value = inputTestValue;
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            Executed = true;
            OutputMessage("Hello from TestNode!");
            return Task.CompletedTask;
        }
    }
}
