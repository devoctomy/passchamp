﻿using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8EncoderNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin PlainText
        {
            get
            {
                return GetInput("PlainText");
            }
            set
            {
                Input["PlainText"] = value;
            }
        }

        [NodeOutputPin]
        public IDataPin EncodedBytes
        {
            get
            {
                return GetOutput("EncodedBytes");
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            EncodedBytes.Value = System.Text.Encoding.UTF8.GetBytes(PlainText.GetValue<string>());
            return Task.CompletedTask;
        }
    }
}
