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
    public sealed class MovieSettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private MovieFull _movie;

        /// <summary>
        /// The movie
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        private SubtitlesViewModel _subtitles;

        /// <summary>
        /// The view model used to manage subtitles
        /// </summary>
        public SubtitlesViewModel Subtitles
        {
            get { return _subtitles; }
            set { Set(() => Subtitles, ref _subtitles, value); }
        }

        private RelayCommand _setSubtitlesCommand;

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
                    if (Subtitles == null)
                    {
                        Subtitles = await SubtitlesViewModel.CreateAsync(Movie);
                    }
                    else
                    {
                        Logger.Info(
                            $"Disabling subtitles for movie: {Movie.Title}");
                        Subtitles.Cleanup();
                        Subtitles = null;
                    }
                }));
            }
        }

        private RelayCommand _downloadMovieCommand;

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

        private RelayCommand _cancelCommand;

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
        /// Initializes a new instance of the MovieSettingsViewModel class.
        /// </summary>
        /// <param name="movie">The movie</param>
        public MovieSettingsViewModel(MovieFull movie)
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