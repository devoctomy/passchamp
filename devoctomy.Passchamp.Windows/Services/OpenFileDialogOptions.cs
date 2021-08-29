namespace devoctomy.Passchamp.Windows.Services
{
    public class OpenFileDialogOptions
    {
        public bool CheckFileExists { get; set; }
        public bool CheckPathExists { get; set; }
        public string DefaultExt { get; set; }
        public string Filter { get; set; }
        public bool Multiselect { get; set; }
        public string InitialDirectory { get; set; }
        public bool AddExtension { get; set; }
    }
}