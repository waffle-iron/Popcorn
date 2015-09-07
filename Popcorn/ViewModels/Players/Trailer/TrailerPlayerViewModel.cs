using System;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;

namespace Popcorn.ViewModels.Players.Trailer
{
    /// <summary>
    /// Manage trailer player
    /// </summary>
    public sealed class TrailerPlayerViewModel : MediaPlayerViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private Models.Trailer.Trailer _trailer;

        /// <summary>
        /// The trailer
        /// </summary>
        public Models.Trailer.Trailer Trailer
        {
            get { return _trailer; }
            private set { Set(() => Trailer, ref _trailer, value); }
        }

        /// <summary>
        /// Initializes a new instance of the TrailerPlayerViewModel class.
        /// </summary>
        /// <param name="trailer">The trailer</param>
        public TrailerPlayerViewModel(Models.Trailer.Trailer trailer)
        {
            RegisterCommands();
            Trailer = trailer;
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            StopPlayingMediaCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new StopPlayingTrailerMessage());
            });
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up TrailerPlayerViewModel");

            OnStoppedPlayingMedia(new EventArgs());

            base.Cleanup();
        }
    }
}