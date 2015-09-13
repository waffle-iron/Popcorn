using GalaSoft.MvvmLight.CommandWpf;
using Popcorn.Models.Localization;

namespace Popcorn.ViewModels.Settings
{
    public interface ISettingsViewModel
    {
        /// <summary>
        /// Command used to initialize asynchronously properties
        /// </summary>
        RelayCommand InitializeAsyncCommand { get; }

        /// <summary>
        /// Language
        /// </summary>
        Language Language { get; set; }

        /// <summary>
        /// Download limit
        /// </summary>
        int DownloadLimit { get; set; }

        /// <summary>
        /// Upload limit
        /// </summary>
        int UploadLimit { get; set; }
    }
}
