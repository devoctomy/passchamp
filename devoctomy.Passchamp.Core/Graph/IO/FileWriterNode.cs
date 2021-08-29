using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Graph.IO
{
    public class FileWriterNode : NodeBase
    {
        [NodeInputPin(ValueType = typeof(byte[]), DefaultValue = default(byte[]))]
        public IDataPin<byte[]> InputData
        {
            get
            {
                return GetInput<byte[]>("InputData");
            }
            set
            {
                Input["InputData"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(bool), DefaultValue = true)]
        public IDataPin<bool> CreateDirectory
        {
            get
            {
                return GetInput<bool>("CreateDirectory");
            }
            set
            {
                Input["CreateDirectory"] = value;
            }
        }

        [NodeInputPin(ValueType = typeof(string), DefaultValue = "")]
        public IDataPin<string> FileName
        {
            get
            {
                return GetInput<string>("FileName");
            }
            set
            {
                Input["FileName"] = value;
            }
        }

        protected override async Task DoExecuteAsync(
            IGraph graph,
            CancellationToken cancellationToken)
        {
            var inputData = InputData.Value;
            var outputFile = new FileInfo(FileName.Value);
            if(!outputFile.Directory.Exists)
            {
                if(!CreateDirectory.Value)
                {
                    throw new DirectoryNotFoundException($"Output directory '{outputFile.Directory.FullName}' not found.");
                }

                OutputMessage($"Creating directory '{outputFile.Directory.FullName}'.");
                outputFile.Directory.Create();
            }

            OutputMessage($"Writing file '{outputFile.FullName}'.");
            await File.WriteAllBytesAsync(
                FileName.Value,
                inputData,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
