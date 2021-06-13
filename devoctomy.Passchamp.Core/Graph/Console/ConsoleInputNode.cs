using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Console
{
    public class ConsoleInputNode : NodeBase
    {
        public DataPin Prompt
        {
            get
            {
                return Input["Prompt"];
            }
            set
            {
                Input["Prompt"] = value;
            }
        }

        public DataPin InputLine
        {
            get
            {
                PrepareOutputDataPin("InputLine");
                return Output["InputLine"];
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
