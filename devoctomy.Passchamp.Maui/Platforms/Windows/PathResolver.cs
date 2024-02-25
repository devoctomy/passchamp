using devoctomy.Passchamp.Maui.IO;
using System.Text.RegularExpressions;

namespace devoctomy.Passchamp.Maui.Pathforms.Android.IO;

public class PathResolver : IPathResolver
{
    public string Resolve(string path)
    {
        MatchCollection matches = Regex.Matches(path, @"\{[^}]*\}");
        foreach (Match match in matches)
        {
            switch (match.Value.Trim('{', '}'))
            {
                case CommonPaths.ExternalCommonAppData:
                    {
                        var appData = FileSystem.AppDataDirectory + "\\";
                        path = path.Replace(match.Value, appData);
                        break;
                    }
                case CommonPaths.AppData:
                    {
                        path = path.Replace(match.Value, "devoctomy\\passchamp\\");
                        break;
                    }
                case CommonPaths.Vaults:
                    {
                        path = path.Replace(match.Value, "vaults\\");
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException($"Path token '{match.Value}' is not implemented.");
                    }
            }
        }

        return path;
    }
}
