using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Models.Movie;
using Popcorn.Properties;
using Popcorn.Services.History;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Pages.Player;

namespace Popcorn.ViewModels.Pages.Home.Movie.Player.Movie
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
        /// The movie to manage
        /// </summary>
        public MovieJson Movie;

        /// <summary>
        /// Initializes a new instance of the MoviePlayerViewModel class.
        /// </summary>
        /// <param name="applicationService">Main view model</param>
        /// <param name="movieService">Movie service</param>
        /// <param name="movieHistoryService">Movie history service</param>
        public MoviePlayerViewModel(IApplicationService applicationService, IMovieService movieService,
            IMovieHistoryService movieHistoryService)
            : base(applicationService, movieService, movieHistoryService)
        {
            RegisterMessages();
            RegisterCommands();
        }

        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">Movie to load</param>
        public void LoadMovie(MovieJson movie)
        {
            Logger.Info(
                $"Loading movie: {movie.Title}.");
            Movie = movie;
            TabName = !string.IsNullOrEmpty(Movie.Title) ? Movie.Title : Resources.PlayingTitleTab;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
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
            =>
                StopPlayingMediaCommand =
                    new RelayCommand(() => { Messenger.Default.Send(new StopPlayingMovieMessage()); });
    }
}