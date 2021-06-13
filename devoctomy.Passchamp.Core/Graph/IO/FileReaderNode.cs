using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileReaderNode : NodeBase
    {
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

        public DataPin Bytes
        {
            get
            {
                PrepareOutputDataPin("Bytes");
                return Output["Bytes"];
            }
        }

        public override async Task Execute(
            Graph graph,
            CancellationToken cancellationToken)
        {
            Output["Bytes"].Value = await System.IO.File.ReadAllBytesAsync(
                FileName.Value as string,
                cancellationToken);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}