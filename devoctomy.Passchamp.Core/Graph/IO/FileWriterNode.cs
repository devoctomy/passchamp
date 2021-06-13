using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileWriterNode : NodeBase
    {
        public DataPin InputData
        {
            get
            {
                return Input["InputData"];
            }
            set
            {
                Input["InputData"] = value;
            }
        }

        public DataPin FileName
        {
            get
            {
                return Input["FileName"];
            }
            set
            {
                Input["FileName"] = value;
            }
        }

        public override async Task Execute(
            Graph graph,
            CancellationToken cancellationToken)
        {
            var inputData = InputData.Value as byte[];
            await System.IO.File.WriteAllBytesAsync(
                FileName.Value as string,
                inputData,
                cancellationToken);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
