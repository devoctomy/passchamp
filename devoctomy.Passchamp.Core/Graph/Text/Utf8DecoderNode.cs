using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8DecoderNode : NodeBase
    {
        public DataPin EncodedBytes
        {
            get
            {
                return Input["EncodedBytes"];
            }
            set
            {
                Input["EncodedBytes"] = value;
            }
        }

        public DataPin PlainText
        {
            get
            {
                PrepareOutputDataPin("PlainText");
                return Output["PlainText"];
            }
            set
            {
                Output["PlainText"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            PlainText.Value = System.Text.Encoding.UTF8.GetString(EncodedBytes.Value as byte[]);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
