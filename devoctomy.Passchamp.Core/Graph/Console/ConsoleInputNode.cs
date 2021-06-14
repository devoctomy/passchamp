using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Console
{
    public class ConsoleInputNode : NodeBase
    {
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
            set
            {
                Output["InputLine"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            System.Console.WriteLine(Prompt.GetValue<string>());
            InputLine.Value = System.Console.ReadLine();

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
