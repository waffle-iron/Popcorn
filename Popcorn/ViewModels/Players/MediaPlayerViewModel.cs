using System;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.ViewModels.Main;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.ViewModels.Players
{
    /// <summary>
    /// Manage media player
    /// </summary>
    public class MediaPlayerViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property -> Main

        /// <summary>
        /// The main view model
        /// </summary>
        public MainViewModel Main { get; }

        #endregion

        #region Commands

        #region Command -> StopPlayingMediaCommand

        /// <summary>
        /// Command used to stop playing the media
        /// </summary>
        public RelayCommand StopPlayingMediaCommand { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MediaPlayerViewModel class.
        /// </summary>
        protected MediaPlayerViewModel()
        {
            Logger.Debug("Initializing a new instance of MediaPlayerViewModel");

            Main = SimpleIoc.Default.GetInstance<MainViewModel>();
        }

        #endregion

        #region Event -> OnStoppedPlayingMedia

        /// <summary>
        /// Event fired on stopped playing the media
        /// </summary>
        public event EventHandler<EventArgs> StoppedPlayingMedia;

        /// <summary>
        /// Fire StoppedPlayingMedia event
        /// </summary>
        ///<param name="e">Event data</param>
        protected void OnStoppedPlayingMedia(EventArgs e)
        {
            Logger.Debug(
                "Stop playing a media");

            var handler = StoppedPlayingMedia;
            handler?.Invoke(this, e);
        }

        #endregion
    }
}