using devoctomy.Passchamp.Core.Graph.Data;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class DataParserSectionParser : IDataParserSectionParser
    {
        public List<DataParserSection> Parse(string parserSections)
        {
            var allSections = new List<DataParserSection>();
            var sections = parserSections.Split(';');
            foreach (var curSection in sections)
            {
                var curSectionParts = curSection.Split(',');
                var startOffset = curSectionParts[1].StartsWith("~") ? Offset.FromEnd : Offset.Absolute;
                var startValue = int.Parse(curSectionParts[1].TrimStart('~'));
                var endOffset = curSectionParts[2].StartsWith("~") ? Offset.FromEnd : Offset.Absolute;
                var endValue = int.Parse(curSectionParts[2].TrimStart('~'));

                var section = new DataParserSection
                {
                    Key = curSectionParts[0],
                    Start = new ArrayLocation(startOffset, startValue),
                    End = new ArrayLocation(endOffset, endValue),
                };
                allSections.Add(section);
            }

            return allSections;
        }
    }
}
