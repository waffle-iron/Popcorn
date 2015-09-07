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
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main view model
        /// </summary>
        public MainViewModel Main { get; }

        /// <summary>
        /// Command used to stop playing the media
        /// </summary>
        public RelayCommand StopPlayingMediaCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MediaPlayerViewModel class.
        /// </summary>
        protected MediaPlayerViewModel()
        {
            if (SimpleIoc.Default.IsRegistered<MainViewModel>())
                Main = SimpleIoc.Default.GetInstance<MainViewModel>();
        }

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
    }
}