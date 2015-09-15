using System;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.History;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Players.Trailer
{
    /// <summary>
    ///     Manage trailer player
    /// </summary>
    public sealed class TrailerPlayerViewModel : MediaPlayerViewModel, ITrailerPlayerViewModel
    {
        /// <summary>
        ///     Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private Models.Trailer.Trailer _trailer;

        /// <summary>
        ///     Initializes a new instance of the TrailerPlayerViewModel class.
        /// </summary>
        /// <param name="applicationState">Main view model</param>
        /// <param name="movieService">Movie service</param>
        /// <param name="movieHistoryService">Movie history service</param>
        public TrailerPlayerViewModel(IApplicationState applicationState, IMovieService movieService,
            IMovieHistoryService movieHistoryService)
            : base(applicationState, movieService, movieHistoryService)
        {
            RegisterCommands();
        }

        /// <summary>
        ///     The trailer
        /// </summary>
        public Models.Trailer.Trailer Trailer
        {
            get { return _trailer; }
            private set { Set(() => Trailer, ref _trailer, value); }
        }

        /// <summary>
        ///     Load a trailer
        /// </summary>
        /// <param name="trailer">Trailer to load</param>
        public void LoadTrailer(Models.Trailer.Trailer trailer)
        {
            Logger.Info(
                $"Loading trailer: {trailer.Uri.AbsoluteUri}.");
            Trailer = trailer;
        }

        /// <summary>
        ///     Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            OnStoppedPlayingMedia(new EventArgs());
            base.Cleanup();
        }

        /// <summary>
        ///     Register commands
        /// </summary>
        private void RegisterCommands() => StopPlayingMediaCommand =
            new RelayCommand(() => { Messenger.Default.Send(new StopPlayingTrailerMessage()); });
    }
}