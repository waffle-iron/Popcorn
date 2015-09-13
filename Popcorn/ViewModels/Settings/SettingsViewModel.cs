using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Popcorn.Models.Localization;
using GalaSoft.MvvmLight.CommandWpf;
using Popcorn.Services.Language;

namespace Popcorn.ViewModels.Settings
{
    /// <summary>
    /// Application's settings
    /// </summary>
    public sealed class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        /// <summary>
        /// Services used to interacts with languages
        /// </summary>
        private readonly ILanguageService _languageService;

        private int _downloadLimit;

        private int _uploadLimit;

        private Language _language;

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel(ILanguageService languageService)
        {
            RegisterCommands();
            _languageService = languageService;
        }

        /// <summary>
        /// Download limit
        /// </summary>
        public int DownloadLimit
        {
            get { return _downloadLimit; }
            set { Set(() => DownloadLimit, ref _downloadLimit, value); }
        }

        /// <summary>
        /// Upload limit
        /// </summary>
        public int UploadLimit
        {
            get { return _uploadLimit; }
            set { Set(() => UploadLimit, ref _uploadLimit, value); }
        }

        /// <summary>
        /// Language
        /// </summary>
        public Language Language
        {
            get { return _language; }
            set { Set(() => Language, ref _language, value); }
        }

        /// <summary>
        /// Command used to initialize asynchronously properties
        /// </summary>
        public RelayCommand InitializeAsyncCommand { get; private set; }

        /// <summary>
        /// Load asynchronously the languages of the application for the current instance
        /// </summary>
        /// <returns>Instance of SettingsViewModel</returns>
        private async Task InitializeAsync()
        {
            Language = new Language(_languageService);
            await Language.LoadLanguages();
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