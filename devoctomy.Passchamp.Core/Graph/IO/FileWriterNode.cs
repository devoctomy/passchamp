using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileWriterNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> InputData
        {
            get
            {
                return GetInput<byte[]>("InputData");
            }
            set
            {
                Input["InputData"] = value;
            }
        }

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

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var inputData = InputData.Value;
            await System.IO.File.WriteAllBytesAsync(
                FileName.Value,
                inputData,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
