using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Models.Movie.Full;
using Popcorn.Services.History;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Players.Movie
{
    /// <summary>
    /// Manage movie player
    /// </summary>
    public sealed class MoviePlayerViewModel : MediaPlayerViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Movie
        /// </summary>
        public MovieFull Movie;

        /// <summary>
        /// Initializes a new instance of the MoviePlayerViewModel class.
        /// </summary>
        /// <param name="applicationState">Main view model</param>
        /// <param name="movieService">Movie service</param>
        /// <param name="movieHistoryService">Movie history service</param>
        public MoviePlayerViewModel(IApplicationState applicationState, IMovieService movieService, IMovieHistoryService movieHistoryService)
            : base(applicationState, movieService, movieHistoryService)
        {
            RegisterMessages();
            RegisterCommands();
        }

        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">Movie to load</param>
        public void LoadMovie(MovieFull movie)
        {
            Movie = movie;
            TabName = !string.IsNullOrEmpty(Movie.Title) ? Movie.Title : Properties.Resources.PlayingTitleTab;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up MoviePlayerViewModel");

            OnStoppedPlayingMedia(new EventArgs());

            base.Cleanup();
        }

        /// <summary>
        /// When a movie has been seen, save this information in user data
        /// </summary>
        public async Task HasSeenMovie()
        {
            await MovieHistoryService.SetHasBeenSeenMovieAsync(Movie);
            Messenger.Default.Send(new ChangeHasBeenSeenMovieMessage());
            Messenger.Default.Send(new StopPlayingMovieMessage());
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                language => { TabName = Movie.Title; });
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            StopPlayingMediaCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new StopPlayingMovieMessage());
            });
        }
    }
}