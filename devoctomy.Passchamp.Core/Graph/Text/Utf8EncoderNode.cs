﻿using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Text
{
    public class Utf8EncoderNode : NodeBase
    {
        public DataPin PlainText
        {
            get
            {
                return Input["PlainText"];
            }
            set
            {
                Input["PlainText"] = value;
            }
        }

        public DataPin EncodedBytes
        {
            get
            {
                PrepareOutputDataPin("EncodedBytes");
                return Output["EncodedBytes"];
            }
            set
            {
                Output["EncodedBytes"] = value;
            }
        }

        public override async Task Execute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            EncodedBytes.Value = System.Text.Encoding.UTF8.GetBytes(PlainText.GetValue<string>());

            await ExecuteNext(
                graph,
                cancellationToken);
        }
    }
}
