using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Text;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Presets.Decrypt;

public class StandardDecrypt(IDataParserSectionParser dataParserSectionParser) : IGraphPreset
{
    public GraphContext Context => GraphContext.Decrypt;
    public bool Default => true;

    public string Author => "devoctomy";

    public string Description => "Default native decryption graph.";

    public string StartKey => null;

    public List<NodeRef> OrderedNodes =>
        [
            new NodeRef
            {
                Key = "streamReader",
                NodeType = typeof(StreamReaderNode),
                InputPins = new Dictionary<string, IPin>
                {
                    { "Stream", DataPinFactory.Instance.Create("Stream", new DataPinIntermediateValue("Pins.InputStream")) },
                }

            },
            new NodeRef
            {
                Key = "dataParser",
                NodeType = typeof(DataParserNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "Sections", DataPinFactory.Instance.Create("Sections", _dataParserSectionParser.Parse("Iv,0,16;Cipher,16,~16;Salt,~16,~0")) },
                    }

            },
            new NodeRef
            {
                Key = "deriveKey",
                NodeType = typeof(DeriveKeyFromPasswordExNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "KeyLength", DataPinFactory.Instance.Create("KeyLength", new DataPinIntermediateValue("Pins.KeyLength")) },
                        { "SecurePassword", DataPinFactory.Instance.Create("SecurePassword", new DataPinIntermediateValue("Pins.Passphrase")) }
                    }
            },
            new NodeRef
            {
                Key = "decrypt",
                NodeType = typeof(DecryptNode)
            },
            new NodeRef
            {
                Key = "decode",
                NodeType = typeof(Utf8DecoderNode),
                InputPins = []
            }
        ];

    public List<NodeRef> UnorderedNodes => null;

    public Dictionary<string, IPin> InputPins =>
        new Dictionary<string, IPin>
            {
                { "KeyLength", DataPinFactory.Instance.Create("KeyLength", 32) },
                { "Passphrase", null },
                { "InputStream", null }
            };

    public Dictionary<string, IPin> OutputPins =>
        new Dictionary<string, IPin>
            {
                { "DecryptedBytes", DataPinFactory.Instance.Create("PlainText", new DataPinIntermediateValue("decrypt.DecryptedBytes"), typeof(DataPinIntermediateValue)) }
            };

    public List<NodeConnection> Connections =>
        [
            new NodeConnection("streamReader", "Bytes", "dataParser", "Bytes"),
            new NodeConnection("dataParser", "DataParserSection:Salt", "deriveKey", "Salt"),
            new NodeConnection("dataParser", "DataParserSection:Iv", "decrypt", "Iv"),
            new NodeConnection("dataParser", "DataParserSection:Cipher", "decrypt", "Cipher"),
            new NodeConnection("deriveKey", "Key", "decrypt", "Key"),
            new NodeConnection("decrypt", "DecryptedBytes", "decode", "EncodedBytes"),
        ];

    private readonly IDataParserSectionParser _dataParserSectionParser = dataParserSectionParser;
}
