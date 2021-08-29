using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Console
{
    public class ConsoleInputNode : NodeBase
    {
        private readonly ISystemConsole _systemConsole;

        public ConsoleInputNode()
        {
        }

        public ConsoleInputNode(ISystemConsole systemConsole)
        {
            _systemConsole = systemConsole;
        }

        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin<string> Prompt
        {
            get
            {
                return GetInput<string>("Prompt");                
            }
            set
            {
                Input["Prompt"] = value;
            }
        }

        [NodeOutputPin]
        public IDataPin<string> InputLine
        {
            get
            {
                return GetOutput<string>("InputLine");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            _systemConsole.WriteLine(Prompt.Value);
            InputLine.Value = _systemConsole.ReadLine();
            return Task.CompletedTask;
        }
    }
}
