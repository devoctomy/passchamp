﻿using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Cryptography
{
    public class RandomByteArrayGeneratorNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(int), DefaultValue = 0)]
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

        [NodeOutputPin]
        public IDataPin RandomBytes
        {
            get
            {
                return GetOutput("RandomBytes");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[Length.GetValue<int>()];
            rng.GetBytes(randomBytes);
            RandomBytes.Value = randomBytes;
            return Task.CompletedTask;
        }
    }
}
