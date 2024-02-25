using Android.AdServices.Common;
using devoctomy.Passchamp.Maui.IO;
using System.Text.RegularExpressions;

namespace devoctomy.Passchamp.Maui.Pathforms.Android.IO;

public class PathResolver : IPathResolver
{
    private IMauiContext _mauiContext;

    public void Initialise(IMauiContext mauiContext)
    {
        _mauiContext = mauiContext;
    }

    public string Resolve(string path)
    {
        if(_mauiContext == null)
        {
            throw new InvalidOperationException("PathResolver has not been initialised for Android platform.");
        }

        MatchCollection matches = Regex.Matches(path, @"\{[^}]*\}");
        foreach (Match match in matches)
        {
            switch (match.Value.Trim('{', '}'))
            {
                case CommonPaths.ExternalCommonAppData:
                    {
                        var appData = _mauiContext.Context.GetExternalFilesDir(null)?.AbsolutePath + "/";
                        path = path.Replace(match.Value, appData);
                        break;
                    }
                case CommonPaths.AppData:
                    {
                        path = path.Replace(match.Value, "devoctomy/passchamp/");
                        break;
                    }
                case CommonPaths.Vaults:
                    {
                        path = path.Replace(match.Value, "vaults/");
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
