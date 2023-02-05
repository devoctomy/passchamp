using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.Data;

public class DataParserNode : NodeBase
{
    private readonly Dictionary<string, IDataPin<byte[]>> _sectionValues = new();

    [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
    public IDataPin<byte[]> Bytes
    {
        get
        {
            return GetInput<byte[]>("Bytes");
        }
        set
        {
            Input["Bytes"] = value;
        }
    }

    [NodeInputPin(ValueType = typeof(List<DataParserSection>), DefaultValue = default(List<DataParserSection>))]
    public IDataPin<List<DataParserSection>> Sections
    {
        get
        {
            return GetInput<List<DataParserSection>>("Sections");
        }
        set
        {
            var sections = value.Value;
            foreach(var curSection in sections)
            {
                _sectionValues.Add(
                    curSection.Key,
                    new DataPin<byte[]>(
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
        Parallel.ForEach(Sections.Value, curSection =>
        {
            var start = curSection.Start.GetIndex(Bytes.Value.Length);
            var end = curSection.End.GetIndex(Bytes.Value.Length);
            var value = new byte[end - start];
            Array.ConstrainedCopy(
                Bytes.Value,
                start,
                value,
                0,
                value.Length);
            GetSectionValue(curSection.Key).Value = value;
        });

        return Task.CompletedTask;
    }

    public IDataPin<byte[]> GetSectionValue(string key)
    {
        return _sectionValues[key];
    }
}