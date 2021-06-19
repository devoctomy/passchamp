using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Data
{
    public class DataParserNode : NodeBase
    {
        private Dictionary<string, DataPin> _sectionValues = new Dictionary<string, DataPin>();

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
                        new DataPin(null));
                }
                Input["Sections"] = value;
            }
        }

        protected override Task DoExecute(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var values = new Dictionary<string, byte[]>();
            foreach(var curSection in Sections.GetValue<List<DataParserSection>>())
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
            }

            return Task.CompletedTask;
        }

        public DataPin GetSectionValue(string key)
        {
            return _sectionValues[key];
        }
    }
}