using System;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.History;
using Popcorn.Services.Movie;
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
        /// Initializes a new instance of the MediaPlayerViewModel class.
        /// </summary>
        protected MediaPlayerViewModel(IApplicationState applicationState, IMovieService movieService,
            IMovieHistoryService movieHistoryService)
            : base(applicationState, movieService, movieHistoryService)
        {
        }

        /// <summary>
        /// Command used to stop playing the media
        /// </summary>
        public RelayCommand StopPlayingMediaCommand { get; set; }

        /// <summary>
        /// Event fired on stopped playing the media
        /// </summary>
        public event EventHandler<EventArgs> StoppedPlayingMedia;

        /// <summary>
        /// Fire StoppedPlayingMedia event
        /// </summary>
        /// <param name="e">Event args</param>
        protected void OnStoppedPlayingMedia(EventArgs e)
        {
            Logger.Debug(
                "Stop playing a media");

            var handler = StoppedPlayingMedia;
            handler?.Invoke(this, e);
        }
    }
}