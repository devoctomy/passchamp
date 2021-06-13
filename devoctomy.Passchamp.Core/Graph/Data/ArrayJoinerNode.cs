using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class ArrayJoinerNode : NodeBase
    {
        public DataPin Part1
        {
            get
            {
                PrepareInputDataPin("Part1");
                return Input["Part1"];
            }
            set
            {
                Input["Part1"] = value;
            }
        }

        public DataPin Part2
        {
            get
            {
                PrepareInputDataPin("Part2");
                return Input["Part2"];
            }
            set
            {
                Input["Part2"] = value;
            }
        }

        public DataPin Part3
        {
            get
            {
                PrepareInputDataPin("Part3");
                return Input["Part3"];
            }
            set
            {
                Input["Part3"] = value;
            }
        }

        public DataPin Part4
        {
            get
            {
                PrepareInputDataPin("Part4");
                return Input["Part4"];
            }
            set
            {
                Input["Part4"] = value;
            }
        }

        public DataPin JoinedOutput
        {
            get
            {
                PrepareOutputDataPin("JoinedOutput");
                return Output["JoinedOutput"];
            }
            set
            {
                Output["JoinedOutput"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var allParts = new List<byte[]>();
            if (Part1.Value != null) allParts.Add(Part1.GetValue<byte[]>());
            if (Part2.Value != null) allParts.Add(Part2.GetValue<byte[]>());
            if (Part3.Value != null) allParts.Add(Part3.GetValue<byte[]>());
            if (Part4.Value != null) allParts.Add(Part4.GetValue<byte[]>());
            JoinedOutput.Value = allParts.SelectMany(x => x).ToArray();

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
