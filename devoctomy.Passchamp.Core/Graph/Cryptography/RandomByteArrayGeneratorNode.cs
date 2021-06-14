using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class RandomByteArrayGeneratorNode : NodeBase
    {
        public IDataPin Length
        {
            get
            {
                return GetInput("Length");
            }
            set
            {
                Input["Length"] = value;
            }
        }

        public IDataPin RandomBytes
        {
            get
            {
                return GetOutput("RandomBytes");
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[Length.GetValue<int>()];
            rng.GetBytes(randomBytes);
            RandomBytes.Value = randomBytes;

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
