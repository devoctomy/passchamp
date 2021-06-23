using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileReaderNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin FileName
        {
            get
            {
                return GetInput("FileName");
            }
            set
            {
                Input["FileName"] = value;
            }
        }

        [NodeOutputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Bytes
        {
            get
            {
                return GetOutput("Bytes");
            }
        }

        protected override async Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            Bytes.Value = await System.IO.File.ReadAllBytesAsync(
                FileName.GetValue<string>(),
                cancellationToken);
        }
    }
}