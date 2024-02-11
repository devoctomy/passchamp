using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.IO;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Text;
using System.Collections.Generic;

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

            InputPins = new Dictionary<string, IPin>
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

    public static GraphPreset DefaultDecrypt()
    {
        var preset = new GraphPreset
        {
            Author = "devoctomy",
            Description = "Default native decryption graph.",
#if DEBUG
            Debug = false,
#endif

            OrderedNodes =
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
                        { "Sections", DataPinFactory.Instance.Create("Sections", ParseDataParserSectionsString("Iv,0,16;Cipher,16,~16;Salt,~16,~0")) },
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
                    InputPins = new Dictionary<string, IPin>
                    {
                    }
                }
            ],

            Connections =
            [
                new NodeConnection("streamReader", "Bytes", "dataParser", "Bytes"),
                new NodeConnection("dataParser", "DataParserSection:Salt", "deriveKey", "Salt"),
                new NodeConnection("dataParser", "DataParserSection:Iv", "decrypt", "Iv"),
                new NodeConnection("dataParser", "DataParserSection:Cipher", "decrypt", "Cipher"),
                new NodeConnection("deriveKey", "Key", "decrypt", "Key"),
                new NodeConnection("decrypt", "DecryptedBytes", "decode", "EncodedBytes"),
            ],

            InputPins = new Dictionary<string, IPin>
            {
                { "KeyLength", DataPinFactory.Instance.Create("KeyLength", 32) },
                { "Passphrase", null },
                { "InputStream", null }
            },

            OutputPins = new Dictionary<string, IPin>
            {
                { "DecryptedBytes", DataPinFactory.Instance.Create("PlainText", new DataPinIntermediateValue("decrypt.DecryptedBytes"), typeof(DataPinIntermediateValue)) }
            }
        };

        return preset;
    }

    private static List<DataParserSection> ParseDataParserSectionsString(string parserSections)
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
