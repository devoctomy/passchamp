using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8DecoderNode : NodeBase
    {
        public IDataPin EncodedBytes
        {
            get
            {
                return GetInput("EncodedBytes");
            }
            set
            {
                Input["EncodedBytes"] = value;
            }
        }

        public IDataPin PlainText
        {
            get
            {
                return GetOutput("PlainText");
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            PlainText.Value = System.Text.Encoding.UTF8.GetString(EncodedBytes.GetValue<byte[]>());

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
