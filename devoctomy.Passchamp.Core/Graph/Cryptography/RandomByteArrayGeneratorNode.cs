using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class RandomByteArrayGeneratorNode : NodeBase
    {
        public DataPin Length
        {
            get
            {
                return Input["Length"];
            }
            set
            {
                Input["Length"] = value;
            }
        }

        public DataPin RandomBytes
        {
            get
            {
                PrepareOutputDataPin("RandomBytes");
                return Output["RandomBytes"];
            }
            set
            {
                Output["RandomBytes"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[(int)Length.Value];
            rng.GetBytes(randomBytes);
            RandomBytes.Value = randomBytes;

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
