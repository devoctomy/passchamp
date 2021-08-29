using Microsoft.Win32;

namespace devoctomy.Passchamp.Windows.Services
{
    public class FileDialogService : IFileDialogService
    {
        public bool? OpenFile(
            OpenFileDialogOptions options,
            out string fileName)
        {
            fileName = null;
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = options.CheckFileExists,
                CheckPathExists = options.CheckPathExists,
                DefaultExt = options.DefaultExt,
                Filter = options.Filter,
                Multiselect = options.Multiselect,
                InitialDirectory = options.InitialDirectory,
                AddExtension = options.AddExtension
            };

            bool? retVal = openFileDialog.ShowDialog();
            if(retVal.HasValue && retVal.Value)
            {
                fileName = openFileDialog.FileName;
            }

            return retVal;
        }
    }
}
