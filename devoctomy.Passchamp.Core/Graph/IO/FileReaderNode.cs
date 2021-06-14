using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileReaderNode : NodeBase
    {
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

        public IDataPin Bytes
        {
            get
            {
                return GetOutput("Bytes");
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            Bytes.Value = await System.IO.File.ReadAllBytesAsync(
                FileName.GetValue<string>(),
                cancellationToken);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}