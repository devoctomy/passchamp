namespace devoctomy.Passchamp.Windows.Services
{
    public interface IFileDialogService
    {
        bool? OpenFile(
            OpenFileDialogOptions options,
            out string fileName);
    }
}
