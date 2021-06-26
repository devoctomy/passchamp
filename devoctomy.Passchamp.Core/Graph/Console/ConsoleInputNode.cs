using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Console
{
    public class ConsoleInputNode : NodeBase
    {
        private readonly ISystemConsole _systemConsole;

        public ConsoleInputNode(ISystemConsole systemConsole)
        {
            _systemConsole = systemConsole;
        }

        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin Prompt
        {
            get
            {
                return GetInput("Prompt");                
            }
            set
            {
                Input["Prompt"] = value;
            }
        }

        [NodeOutputPin]
        public IDataPin InputLine
        {
            get
            {
                return GetOutput("InputLine");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            _systemConsole.WriteLine(Prompt.GetValue<string>());
            InputLine.Value = _systemConsole.ReadLine();
            return Task.CompletedTask;
        }
    }
}
