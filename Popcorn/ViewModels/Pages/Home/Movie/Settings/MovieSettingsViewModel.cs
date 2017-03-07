using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Messaging;
using Popcorn.Models.Movie;
using Popcorn.ViewModels.Pages.Home.Movie.Subtitles;

namespace Popcorn.ViewModels.Pages.Home.Movie.Settings
{
    /// <summary>
    /// Manage the movie's playing settings
    /// </summary>
    public sealed class MovieSettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Command used to cancel the settings
        /// </summary>
        private RelayCommand _cancelCommand;

        /// <summary>
        /// Command used to download the movie
        /// </summary>
        private RelayCommand _downloadMovieCommand;

        /// <summary>
        /// The movie to manage
        /// </summary>
        private MovieJson _movie;

        /// <summary>
        /// Command used to set the movie's subtitles
        /// </summary>
        private RelayCommand _setSubtitlesCommand;

        /// <summary>
        /// Manage the movie's subtitles
        /// </summary>
        private SubtitlesViewModel _subtitles;

        /// <summary>
        /// Command used to unset the movie's subtitles
        /// </summary>
        private RelayCommand _unSetSubtitlesCommand;

        /// <summary>
        /// Initializes a new instance of the MovieSettingsViewModel class.
        /// </summary>
        /// <param name="subtitles">The subtitles view model</param>
        public MovieSettingsViewModel(SubtitlesViewModel subtitles)
        {
            Subtitles = subtitles;
        }

        /// <summary>
        /// The movie to manage
        /// </summary>
        public MovieJson Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// Manage the movie's subtitles
        /// </summary>
        public SubtitlesViewModel Subtitles
        {
            get { return _subtitles; }
            set { Set(() => Subtitles, ref _subtitles, value); }
        }

        /// <summary>
        /// Comand used to set the movie's subtitles
        /// </summary>
        public RelayCommand SetSubtitlesCommand => _setSubtitlesCommand ??
                                                   (_setSubtitlesCommand =
                                                       new RelayCommand(
                                                           async () => { await Subtitles.LoadSubtitlesAsync(Movie); }));

        /// <summary>
        /// Command used to unset the movie's subtitles
        /// </summary>
        public RelayCommand UnSetSubtitlesCommand
            => _unSetSubtitlesCommand ?? (_unSetSubtitlesCommand = new RelayCommand(
                () => Subtitles.ClearSubtitles()));

        /// <summary>
        /// Command used to download the movie
        /// </summary>
        public RelayCommand DownloadMovieCommand
            =>
                _downloadMovieCommand ??
                (_downloadMovieCommand =
                    new RelayCommand(() => { Messenger.Default.Send(new DownloadMovieMessage(Movie)); }));

        /// <summary>
        /// Command used to cancel the settings
        /// </summary>
        public RelayCommand CancelCommand
            =>
                _cancelCommand ??
                (_cancelCommand = new RelayCommand(() => { Messenger.Default.Send(new StopPlayingMovieMessage()); }));

        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        public void LoadMovie(MovieJson movie)
        {
            movie.FullHdAvailable = movie.Torrents.Any(torrent => torrent.Quality == "1080p");
            Movie = movie;
            Subtitles.ClearSubtitles();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Subtitles?.Cleanup();
            base.Cleanup();
        }
    }
}