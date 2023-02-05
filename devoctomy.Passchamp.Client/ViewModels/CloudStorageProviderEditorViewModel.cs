using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using devoctomy.Passchamp.Client.ViewModels.Base;
using devoctomy.Passchamp.Client.ViewModels.Enums;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;

namespace devoctomy.Passchamp.Client.ViewModels;

public partial class CloudStorageProviderEditorViewModel : BaseViewModel
{
    public string Id { get; set; }

    [ObservableProperty]
    private PageEditorMode editorMode;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string accessId;

    [ObservableProperty]
    private string secretKey;

    [ObservableProperty]
    private string region = Amazon.RegionEndpoint.EUWest2.DisplayName;

    [ObservableProperty]
    private string bucket;

    [ObservableProperty]
    private string path;

    [ObservableProperty]
    private List<string> regions = Amazon.RegionEndpoint.EnumerableAllRegions.Select(x => x.DisplayName).ToList();

    [ObservableProperty]
    private bool okCommandEnabled;

    public BaseViewModel ReturnViewModel { get; set; }

    public IAsyncRelayCommand BackCommand { get; private set; }
    public IAsyncRelayCommand OkCommand { get; private set; }
    public IAsyncRelayCommand ValidateInputCommand { get; private set; }

    public CloudStorageProviderEditorViewModel(BaseViewModel returnViewModel)
    {
        AttachCommandHandlers();
        AttachValidators();

        EditorMode = PageEditorMode.Create;
        ReturnViewModel = returnViewModel;
    }

    public CloudStorageProviderEditorViewModel(
        AmazonS3CloudStorageProviderConfig amazonS3CloudStorageProviderConfig,
        BaseViewModel returnViewModel)
    {
        AttachCommandHandlers();
        AttachValidators();

        EditorMode = PageEditorMode.Edit;
        Id = amazonS3CloudStorageProviderConfig.Id;
        DisplayName = amazonS3CloudStorageProviderConfig.DisplayName;
        AccessId = amazonS3CloudStorageProviderConfig.AccessId;
        SecretKey = amazonS3CloudStorageProviderConfig.SecretKey;
        Region = amazonS3CloudStorageProviderConfig.Region;
        Bucket = amazonS3CloudStorageProviderConfig.Bucket;
        Path = amazonS3CloudStorageProviderConfig.Path;

        ReturnViewModel = returnViewModel;
    }

    private void AttachCommandHandlers()
    {
        BackCommand = new AsyncRelayCommand(BackCommandHandler);
        OkCommand = new AsyncRelayCommand(OkCommandHandler);
    }

    private void AttachValidators()
    {
        ValidateInputCommand = new AsyncRelayCommand(ValidateInputCommandHandler, OkCommandCanExecute);

        OkCommand.NotifyCanExecuteChanged();
    }

    private async Task BackCommandHandler()
    {
        await ReturnViewModel.Return(null);
    }

    private async Task OkCommandHandler()
    {
        await ReturnViewModel.Return(this);
    }

    private bool OkCommandCanExecute()
    {
        var validateInput = () =>
        {
            if (DisplayName == null || DisplayName.Length == 0)
            {
                return false;
            }

            if (AccessId == null || AccessId.Length == 0)
            {
                return false;
            }

            if (SecretKey == null || SecretKey.Length == 0)
            {
                return false;
            }

            if (Region == null || Region.Length == 0)
            {
                return false;
            }

            if (Bucket == null || Bucket.Length == 0)
            {
                return false;
            }

            if (Path == null || Path.Length == 0)
            {
                return false;
            }

            return true;
        };

        OkCommandEnabled = validateInput();
        return OkCommandEnabled;
    }

    private Task ValidateInputCommandHandler()
    {
        OkCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}
