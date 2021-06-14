using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class ArrayJoinerNode : NodeBase
    {
        public IDataPin Part1
        {
            get
            {
                return GetInput("Part1");
            }
            set
            {
                Input["Part1"] = value;
            }
        }

        public IDataPin Part2
        {
            get
            {
                return GetInput("Part2");
            }
            set
            {
                Input["Part2"] = value;
            }
        }

        public IDataPin Part3
        {
            get
            {
                return GetInput("Part3");
            }
            set
            {
                Input["Part3"] = value;
            }
        }

        public IDataPin Part4
        {
            get
            {
                return GetInput("Part4");
            }
            set
            {
                Input["Part4"] = value;
            }
        }

        public IDataPin JoinedOutput
        {
            get
            {
                return GetOutput("JoinedOutput");
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
