using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.Movie.Full;

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
        public readonly MovieFull Movie;

        /// <summary>
        /// Initializes a new instance of the MoviePlayerViewModel class.
        /// </summary>
        /// <param name="movie">Movie to play</param>
        public MoviePlayerViewModel(MovieFull movie)
        {
            RegisterMessages();
            RegisterCommands();
            Movie = movie;
            TabName = !string.IsNullOrEmpty(Movie.Title) ? Movie.Title : Properties.Resources.PlayingTitleTab;
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
    }
}