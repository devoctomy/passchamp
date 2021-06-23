using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8DecoderNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
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

        [NodeOutputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin PlainText
        {
            get
            {
                return GetOutput("PlainText");
            }
        }

        protected override Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            PlainText.Value = System.Text.Encoding.UTF8.GetString(EncodedBytes.GetValue<byte[]>());
            return Task.CompletedTask;
        }
    }
}
