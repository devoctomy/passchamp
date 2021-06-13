using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class DeriveKeyFromPasswordNode : NodeBase
    {
        public DataPin Password
        {
            get
            {
                return Input["Password"];
            }
            set
            {
                Input["Password"] = value;
            }
        }

        public DataPin Salt
        {
            get
            {
                return Input["Salt"];
            }
            set
            {
                Input["Salt"] = value;
            }
        }

        public DataPin KeyLength
        {
            get
            {
                return Input["KeyLength"];
            }
            set
            {
                Input["KeyLength"] = value;
            }
        }

        public DataPin Key
        {
            get
            {
                PrepareOutputDataPin("Key");
                return Output["Key"];
            }
            set
            {
                Output["Key"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var crypto = new System.Security.Cryptography.Rfc2898DeriveBytes(
                Password.Value as string,
                Salt.Value as byte[]);

            Key.Value = crypto.GetBytes((int)KeyLength.Value);

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
