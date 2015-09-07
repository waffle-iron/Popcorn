using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Popcorn.Models.Localization;
using GalaSoft.MvvmLight.CommandWpf;

namespace Popcorn.ViewModels.Settings
{
    /// <summary>
    /// Application's settings
    /// </summary>
    public sealed class SettingsViewModel : ViewModelBase
    {
        private int _downloadLimit;

        /// <summary>
        /// Download limit
        /// </summary>
        public int DownloadLimit
        {
            get { return _downloadLimit; }
            set { Set(() => DownloadLimit, ref _downloadLimit, value); }
        }

        private int _uploadLimit;

        /// <summary>
        /// Upload limit
        /// </summary>
        public int UploadLimit
        {
            get { return _uploadLimit; }
            set { Set(() => UploadLimit, ref _uploadLimit, value); }
        }

        private Language _language;

        /// <summary>
        /// Language
        /// </summary>
        public Language Language
        {
            get { return _language; }
            private set { Set(() => Language, ref _language, value); }
        }

        /// <summary>
        /// Command used to initialize asynchronously properties
        /// </summary>
        public RelayCommand InitializeAsyncCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            RegisterCommands();
        }

        /// <summary>
        /// Load asynchronously the languages of the application for the current instance
        /// </summary>
        /// <returns>Instance of SettingsViewModel</returns>
        private async Task InitializeAsync()
        {
            Language = await Language.CreateAsync();
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            InitializeAsyncCommand = new RelayCommand(async () => await InitializeAsync());
        }
    }
}