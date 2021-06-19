using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8EncoderNode : NodeBase
    {
        public IDataPin PlainText
        {
            get
            {
                return GetInput("PlainText");
            }
            set
            {
                Input["PlainText"] = value;
            }
        }

        public IDataPin EncodedBytes
        {
            get
            {
                return GetOutput("EncodedBytes");
            }
        }

        protected override async Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            EncodedBytes.Value = System.Text.Encoding.UTF8.GetBytes(PlainText.GetValue<string>());
        }
    }
}
