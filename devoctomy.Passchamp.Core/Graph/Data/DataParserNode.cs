using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class DataParserNode : NodeBase
    {
        private readonly Dictionary<string, DataPin> _sectionValues = new();

        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin Bytes
        {
            get
            {
                PrepareInputDataPin("Bytes");
                return Input["Bytes"];
            }
            set
            {
                Input["Bytes"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(List<DataParserSection>), DefaultValue = default(List<DataParserSection>))]
        public IDataPin Sections
        {
            get
            {
                PrepareInputDataPin("Sections");
                return Input["Sections"];
            }
            set
            {
                var sections = value.GetValue<List<DataParserSection>>();
                foreach(var curSection in sections)
                {
                    _sectionValues.Add(
                        curSection.Key,
                        new DataPin(
                            curSection.Key,
                            null));
                }
                Input["Sections"] = value;
            }
        }

        protected override Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            Parallel.ForEach(Sections.GetValue<List<DataParserSection>>(), curSection =>
            {
                var start = curSection.Start.GetIndex(Bytes.GetValue<byte[]>().Length);
                var end = curSection.End.GetIndex(Bytes.GetValue<byte[]>().Length);
                var value = new byte[end - start];
                Array.ConstrainedCopy(
                    Bytes.GetValue<byte[]>(),
                    start,
                    value,
                    0,
                    value.Length);
                GetSectionValue(curSection.Key).Value = value;
            });

            return Task.CompletedTask;
        }

        public DataPin GetSectionValue(string key)
        {
            return _sectionValues[key];
        }
    }
}