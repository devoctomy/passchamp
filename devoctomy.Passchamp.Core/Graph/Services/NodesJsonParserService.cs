using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class NodesJsonParserService : INodesJsonParserService
    {
        public Dictionary<string, INode> Parse(
            JArray json,
            out string startNodeKey)
        {
            var nodes = new Dictionary<string, INode>();
            var firstNodeJson = json.First() as JObject;
            startNodeKey = DoParse(
                nodes,
                firstNodeJson).Key;
            return nodes;
        }

        private static KeyValuePair<string, INode> DoParse(
            Dictionary<string, INode> nodes,
            JObject curNode)
        {
            var type = curNode["Type"].Value<string>(); 
            var node = typeof(NodesJsonParserService).Assembly.CreateInstance(type);
            if(node == null)
            {
                throw new TypeLoadException($"Could not activate instance of '{type}'.");
            }
            var inode = node as INode;

            var inputsJson = curNode["Inputs"].Value<JArray>();
            foreach(var curInputJson in inputsJson)
            {
                var key = curInputJson["Key"].Value<string>();
                inode.PrepareInputDataPin(key, typeof(string));
                inode.Input[key] = DataPinFactory.Instance.Create(
                    key,
                    new DataPinIntermediateValue(curInputJson["Value"].Value<string>()),
                    typeof(DataPinIntermediateValue));
            }

            var curNodeKey = curNode["Key"].Value<string>();
            nodes.Add(
                curNodeKey,
                inode);

            if (curNode.ContainsKey("Next"))
            {
                var nextNode = curNode["Next"].Value<JObject>();
                inode.NextKey =  DoParse(
                    nodes,
                    nextNode).Key;
            }

            return new KeyValuePair<string, INode>(
                curNodeKey,
                inode);
        }
    }
}
