using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileWriterNode : NodeBase
    {
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

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var inputData = InputData.GetValue<byte[]>();
            await System.IO.File.WriteAllBytesAsync(
                FileName.GetValue<string>(),
                inputData,
                cancellationToken);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
