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