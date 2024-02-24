using devoctomy.Passchamp.Core.Enums;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Text;
using devoctomy.Passchamp.Core.Graph.Vault;
using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Presets.Encrypt;

public class StandardEncrypt : IGraphPreset
{
    public Version Version => new(1, 0);
    public GraphContext Context => GraphContext.Encrypt;
    public string Id => "ED387AE6-7CD4-4D86-9F91-DDB101E55DDD";
    public bool Default => true;

    public string Author => "devoctomy";

    public string Description => "Default native encryption graph.";

    public string StartKey => null;

    public List<NodeRef> OrderedNodes =>
        [
            new NodeRef
            {
                Key = "saltGenerator",
                NodeType = typeof(RandomByteArrayGeneratorNode),
                InputPins = new Dictionary<string, IPin>
                {
                    { "Length", DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.SaltLength")) }
                }
            },
            new NodeRef
            {
                Key = "ivGenerator",
                NodeType = typeof(RandomByteArrayGeneratorNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "Length", DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.IvLength")) }
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
                Key = "serialiseVault",
                NodeType = typeof(VaultSerialiserNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "Vault", DataPinFactory.Instance.Create("Vault", new DataPinIntermediateValue("Pins.Vault")) }
                    }
            },
            new NodeRef
            {
                Key = "encode",
                NodeType = typeof(Utf8EncoderNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "PlainText", DataPinFactory.Instance.Create("PlainText", new DataPinIntermediateValue("Pins.PlainText")) },
                    }
            },
            new NodeRef
            {
                Key = "encrypt",
                NodeType = typeof(EncryptNode)
            },
            new NodeRef
            {
                Key = "arrayJoiner",
                NodeType = typeof(ArrayJoinerNode)
            },
            new NodeRef
            {
                Key = "streamWriter",
                NodeType = typeof(StreamWriterNode),
                InputPins = new Dictionary<string, IPin>
                    {
                        { "Stream", DataPinFactory.Instance.Create("Stream", new DataPinIntermediateValue("Pins.OutputStream")) },
                    }

            }
        ];

    public List<NodeRef> UnorderedNodes => null;

    public Dictionary<string, IPin> InputPins =>
        new()
        {
                { "SaltLength", DataPinFactory.Instance.Create("SaltLength", 16) },
                { "IvLength", DataPinFactory.Instance.Create("IvLength", 16) },
                { "KeyLength", DataPinFactory.Instance.Create("KeyLength", 32) },
                { "Passphrase", null },
                { "OutputStream", null },
                { "Vault", null }
            };

    public Dictionary<string, IPin> OutputPins => null;

    public List<NodeConnection> Connections =>
        [
            new NodeConnection("saltGenerator", "RandomBytes", "deriveKey", "Salt"),
            new NodeConnection("serialiseVault", "VaultJson", "encode", "PlainText"),
            new NodeConnection("encode", "EncodedBytes", "encrypt", "PlainTextBytes"),
            new NodeConnection("ivGenerator", "RandomBytes", "encrypt", "Iv"),
            new NodeConnection("deriveKey", "Key", "encrypt", "Key"),
            new NodeConnection("ivGenerator", "RandomBytes", "arrayJoiner", "Part1"),
            new NodeConnection("encrypt", "EncryptedBytes", "arrayJoiner", "Part2"),
            new NodeConnection("saltGenerator", "RandomBytes", "arrayJoiner", "Part3"),
            new NodeConnection("arrayJoiner", "JoinedOutput", "streamWriter", "InputData")
        ];

    public override string ToString()
    {
        return Description;
    }
}
