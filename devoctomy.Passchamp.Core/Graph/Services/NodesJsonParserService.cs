using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace devoctomy.Passchamp.Core.Graph.Services
{
    public class NodesJsonParserService : INodesJsonParserService
    {
        private IServiceProvider _serviceProvider;

        public NodesJsonParserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Dictionary<string, INode> Parse(
            JArray json,
            out string startNodeKey)
        {
            var nodes = new Dictionary<string, INode>();
            var firstNodeJson = json.First() as JObject;
            startNodeKey = DoParse(
                nodes,
                firstNodeJson,
                _serviceProvider).Key;
            return nodes;
        }

        private static KeyValuePair<string, INode> DoParse(
            Dictionary<string, INode> nodes,
            JObject curNode,
            IServiceProvider serviceProvider)
        {
            var type = curNode["Type"].Value<string>().Split(":");
            var nodeAssembly = Assembly.Load(type[0]);
            var nodeType = nodeAssembly.GetType(type[1]);
            var node = type.Length == 2 ? serviceProvider.GetService(nodeType) : null;
            if (node == null)
            {
                throw new TypeLoadException($"Could not activate instance of '{type}'.");
            }
            var inode = node as INode;

            var inputsJson = curNode["Inputs"].Value<JArray>();
            foreach(var curInputJson in inputsJson)
            {
                var key = curInputJson["Key"].Value<string>();
                inode.PrepareInputDataPin(key, typeof(string), true);
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
                if(nextNode.Children().Count() > 0)
                {
                    inode.NextKey = DoParse(
                        nodes,
                        nextNode,
                        serviceProvider).Key;
                }
            }

            return new KeyValuePair<string, INode>(
                curNodeKey,
                inode);
        }
    }
}
