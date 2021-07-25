using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileReaderNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin<string> FileName
        {
            get
            {
                return GetInput<string>("FileName");
            }
            set
            {
                Input["FileName"] = value;
            }
        }

        [NodeOutputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Bytes
        {
            get
            {
                return GetOutput<byte[]>("Bytes");
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            Bytes.Value = await System.IO.File.ReadAllBytesAsync(
                FileName.Value,
                cancellationToken).ConfigureAwait(false);
        }
    }
}