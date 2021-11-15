using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class ArrayJoinerNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Part1
        {
            get
            {
                return GetInput<byte[]>("Part1");
            }
            set
            {
                Input["Part1"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Part2
        {
            get
            {
                return GetInput<byte[]>("Part2");
            }
            set
            {
                Input["Part2"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Part3
        {
            get
            {
                return GetInput<byte[]>("Part3");
            }
            set
            {
                Input["Part3"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> Part4
        {
            get
            {
                return GetInput<byte[]>("Part4");
            }
            set
            {
                Input["Part4"] = value;
            }
        }

        [NodeOutputPin(ValueType = typeof(byte[]))]
        public IDataPin<byte[]> JoinedOutput
        {
            get
            {
                return GetOutput<byte[]>("JoinedOutput");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var allParts = new List<byte[]>();
            AddPart(allParts, Part1.Value);
            AddPart(allParts, Part2.Value);
            AddPart(allParts, Part3.Value);
            AddPart(allParts, Part4.Value);
            JoinedOutput.Value = allParts.SelectMany(x => x).ToArray();
            return Task.CompletedTask;
        }

        private void AddPart(
            List<byte[]> parts,
            byte[] value)
        {
            if (value != null)
            {
                parts.Add(value);
            }
        }
    }
}
