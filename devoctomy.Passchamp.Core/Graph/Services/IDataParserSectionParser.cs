using devoctomy.Passchamp.Core.Graph.Data;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services;

public interface IDataParserSectionParser
{
    List<DataParserSection> Parse(string parserSections);
}
