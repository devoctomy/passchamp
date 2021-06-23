using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileWriterNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin InputData
        {
            get
            {
                return GetInput("InputData");
            }
            set
            {
                Input["InputData"] = value;
            }
        }

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

        protected override async Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var inputData = InputData.GetValue<byte[]>();
            await System.IO.File.WriteAllBytesAsync(
                FileName.GetValue<string>(),
                inputData,
                cancellationToken);
        }
    }
}
