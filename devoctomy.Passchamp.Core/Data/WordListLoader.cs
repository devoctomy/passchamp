using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public class WordListLoader : IWordListLoader
    {
        private readonly WordListLoaderConfig _config;
        private Dictionary<string, List<string>> _cachedLists;

        public WordListLoader(WordListLoaderConfig config)
        {
            _config = config;
        }

        public async Task<Dictionary<string, List<string>>> LoadAllAsync(CancellationToken cancellationToken)
        {
            if(_cachedLists == null)
            {
                _cachedLists = new Dictionary<string, List<string>>();
                var allListFiles = Directory.GetFiles(_config.Path, _config.Pattern);
                foreach (var curListFile in allListFiles)
                {
                    var name = new FileInfo(curListFile).Name.Replace(_config.Pattern.TrimStart('*'), string.Empty);
                    var allLines = await File.ReadAllLinesAsync(curListFile, cancellationToken);
                    _cachedLists.Add(name, new List<string>(allLines));
                }
            }

            return _cachedLists;
        }
    }
}
