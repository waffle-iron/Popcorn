using GalaSoft.MvvmLight.CommandWpf;
using Popcorn.Models.Localization;

namespace Popcorn.ViewModels.Settings
{
    public interface ISettingsViewModel
    {
        /// <summary>
        /// Command used to initialize the settings asynchronously
        /// </summary>
        RelayCommand InitializeAsyncCommand { get; }

        /// <summary>
        /// Language
        /// </summary>
        Language Language { get; set; }

        /// <summary>
        /// The download limit
        /// </summary>
        int DownloadLimit { get; set; }

        /// <summary>
        /// The upload limit
        /// </summary>
        int UploadLimit { get; set; }
    }
}