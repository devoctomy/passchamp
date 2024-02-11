using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Text;

namespace devoctomy.Passchamp.Core.Graph.Presets;

public static class NativePresets
{
    public static GraphPreset DefaultEncrypt()
    {
        var preset = new GraphPreset
        {
            Author = "devoctomy",
            Description = "Default native encryption graph.",
#if DEBUG
            Debug = false,
#endif

            OrderedNodes =
            [
                new NodeRef
                {
                    Key = "saltGenerator",
                    NodeType = typeof(RandomByteArrayGeneratorNode),
                    InputPins = new System.Collections.Generic.Dictionary<string, IPin>
                    {
                        { "Length", DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.SaltLength")) }
                    }
                },
                new NodeRef
                {
                    Key = "ivGenerator",
                    NodeType = typeof(RandomByteArrayGeneratorNode),
                    InputPins = new System.Collections.Generic.Dictionary<string, IPin>
                    {
                        { "Length", DataPinFactory.Instance.Create("Length", new DataPinIntermediateValue("Pins.IvLength")) }
                    }
                },
                new NodeRef
                {
                    Key = "deriveKey",
                    NodeType = typeof(DeriveKeyFromPasswordExNode),
                    InputPins = new System.Collections.Generic.Dictionary<string, IPin>
                    {
                        { "KeyLength", DataPinFactory.Instance.Create("KeyLength", new DataPinIntermediateValue("Pins.KeyLength")) },
                        { "SecurePassword", DataPinFactory.Instance.Create("SecurePassword", new DataPinIntermediateValue("Pins.Passphrase")) }
                    }
                },
                new NodeRef
                {
                    Key = "encode",
                    NodeType = typeof(Utf8EncoderNode),
                    InputPins = new System.Collections.Generic.Dictionary<string, IPin>
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
                    InputPins = new System.Collections.Generic.Dictionary<string, IPin>
                    {
                        { "Stream", DataPinFactory.Instance.Create("Stream", new DataPinIntermediateValue("Pins.OutputStream")) },
                    }

                }
            ],

                Connections =
            [
                new NodeConnection("saltGenerator", "RandomBytes", "deriveKey", "Salt"),
                new NodeConnection("encode", "EncodedBytes", "encrypt", "PlainTextBytes"),
                new NodeConnection("ivGenerator", "RandomBytes", "encrypt", "Iv"),
                new NodeConnection("deriveKey", "Key", "encrypt", "Key"),
                new NodeConnection("ivGenerator", "RandomBytes", "arrayJoiner", "Part1"),
                new NodeConnection("encrypt", "EncryptedBytes", "arrayJoiner", "Part2"),
                new NodeConnection("saltGenerator", "RandomBytes", "arrayJoiner", "Part3"),
                new NodeConnection("arrayJoiner", "JoinedOutput", "streamWriter", "InputData")
            ],

            InputPins = new System.Collections.Generic.Dictionary<string, IPin>
            {
                { "SaltLength", DataPinFactory.Instance.Create("SaltLength", 16) },
                { "IvLength", DataPinFactory.Instance.Create("IvLength", 16) },
                { "KeyLength", DataPinFactory.Instance.Create("KeyLength", 32) },
                { "Passphrase", null },
                { "OutputStream", null },
                { "PlainText", null }
            }
        };

        return preset;
    }
}
