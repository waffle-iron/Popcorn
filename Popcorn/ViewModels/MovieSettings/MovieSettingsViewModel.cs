using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.Movie.Full;
using Popcorn.ViewModels.Subtitles;

namespace Popcorn.ViewModels.MovieSettings
{
    /// <summary>
    /// Manage the movie's playing settings
    /// </summary>
    public sealed class MovieSettingsViewModel : ViewModelBase, IMovieSettingsViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private MovieFull _movie;

        private ISubtitlesViewModel _subtitles;

        private RelayCommand _setSubtitlesCommand;

        private RelayCommand _unSetSubtitlesCommand;

        private RelayCommand _downloadMovieCommand;

        private RelayCommand _cancelCommand;

        /// <summary>
        /// Initializes a new instance of the MovieSettingsViewModel class.
        /// </summary>
        /// <param name="subtitles">The subtitles view model</param>
        public MovieSettingsViewModel(ISubtitlesViewModel subtitles)
        {
            Subtitles = subtitles;
        }

        /// <summary>
        /// The movie
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// The view model used to manage subtitles
        /// </summary>
        public ISubtitlesViewModel Subtitles
        {
            get { return _subtitles; }
            set { Set(() => Subtitles, ref _subtitles, value); }
        }

        /// <summary>
        /// Used to enable or disable subtitles
        /// </summary>
        public RelayCommand SetSubtitlesCommand
        {
            get
            {
                return _setSubtitlesCommand ?? (_setSubtitlesCommand = new RelayCommand(async () =>
                {
                    Logger.Info(
                        $"Setting subtitles for movie: {Movie.Title}");

                    Movie.SelectedSubtitle = null;
                    await Subtitles.LoadSubtitlesAsync(Movie);
                }));
            }
        }

        /// <summary>
        /// Used to enable or disable subtitles
        /// </summary>
        public RelayCommand UnSetSubtitlesCommand
        {
            get
            {
                return _unSetSubtitlesCommand ?? (_unSetSubtitlesCommand = new RelayCommand(() =>
                {
                    Logger.Info(
                        $"Disabling subtitles for movie: {Movie.Title}");

                    Movie.SelectedSubtitle = null;
                    Subtitles.Cleanup();
                }));
            }
        }

        /// <summary>
        /// Command used to download the movie
        /// </summary>
        public RelayCommand DownloadMovieCommand
        {
            get
            {
                return _downloadMovieCommand ?? (_downloadMovieCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new DownloadMovieMessage(Movie));
                }));
            }
        }

        /// <summary>
        /// Command used to cancel the download of a movie
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new StopPlayingMovieMessage());
                }));
            }
        }

        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie"></param>
        public void LoadMovie(MovieFull movie)
        {
            Movie = movie;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up MovieSettingsViewModel");

            Subtitles?.Cleanup();
            base.Cleanup();
        }
    }
}