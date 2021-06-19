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

        public IDataPin InputLine
        {
            get
            {
                return GetInput("InputLine");
            }
        }

        protected override async Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            _systemConsole.WriteLine(Prompt.GetValue<string>());
            InputLine.Value = _systemConsole.ReadLine();
        }
    }
}
