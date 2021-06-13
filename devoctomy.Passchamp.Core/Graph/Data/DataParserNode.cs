using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class DataParserNode : NodeBase
    {
        private Dictionary<string, DataPin> _sectionValues = new Dictionary<string, DataPin>();

        public DataPin Bytes
        {
            get
            {
                return Input["Bytes"];
            }
            set
            {
                Input["Bytes"] = value;
            }
        }

        public DataPin Sections
        {
            get
            {
                return Input["Sections"];
            }
            set
            {
                var sections = value.Value as List<DataParserSection>;
                foreach(var curSection in sections)
                {
                    _sectionValues.Add(
                        curSection.Key,
                        new DataPin(null));
                }
                Input["Sections"] = value;
            }
        }

        public override async Task Execute(
            Graph graph,
            CancellationToken cancellationToken)
        {
            var values = new Dictionary<string, byte[]>();
            foreach(var curSection in Sections.Value as List<DataParserSection>)
            {
                var start = curSection.Start.GetIndex(((byte[])Bytes.Value).Length);
                var end = curSection.End.GetIndex(((byte[])Bytes.Value).Length);
                var value = new byte[end - start];
                Array.ConstrainedCopy(
                    ((byte[])Bytes.Value),
                    start,
                    value,
                    0,
                    value.Length);
                GetSectionValue(curSection.Key).Value = value;
            }

            await ExecuteNext(
                graph,
                cancellationToken);
        }

        public DataPin GetSectionValue(string key)
        {
            return _sectionValues[key];
        }
    }
}