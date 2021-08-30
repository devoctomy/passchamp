using devoctomy.Passchamp.Core.Graph.Data;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services.GraphPinPrepFunctions
{
    public class DataParserSectionsParserPinPrepFunction : IGraphPinPrepFunction
    {
        private readonly IDataParserSectionParser _sectionsParser;

        public DataParserSectionsParserPinPrepFunction(IDataParserSectionParser sectionsParser)
        {
            _sectionsParser = sectionsParser;
        }

        public IPin Execute(
            string curNodeKey,
            string value,
            IReadOnlyDictionary<string, IPin> inputPins,
            IReadOnlyDictionary<string, INode> nodes)
        {
            var pathParts = value.Split(".");
            var parserSections = string.Empty;
            if(pathParts[1] == "Pins")
            {
                parserSections = inputPins[pathParts[2]].ObjectValue.ToString();
            }
            else
            {
                parserSections = pathParts[1];
            }

            var allSections = _sectionsParser.Parse(parserSections);

            var curNode = nodes[curNodeKey] as DataParserNode;
            if (curNode == null)
            {
                throw new InvalidOperationException($"Node '{curNodeKey}' is not of type DataParserNode.");
            }
            curNode.Sections = (IDataPin<List<DataParserSection>>)DataPinFactory.Instance.Create(
                    "Sections",
                    allSections);

            return null;
        }

        public bool IsApplicable(string key)
        {
            return key.Equals(
                "ParseDataParserSections",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
