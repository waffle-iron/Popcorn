using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Models.Movie.Full;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Subtitles
{
    public sealed class SubtitlesViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The service used to interact with movies
        /// </summary>
        private MovieService MovieService { get; }

        private MovieFull _movie;

        /// <summary>
        /// The movie
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// Token to cancel downloading subtitles
        /// </summary>
        private CancellationTokenSource CancellationDownloadingSubtitlesToken { get; set; }

        /// <summary>
        /// Initializes a new instance of the SubtitlesViewModel class.
        /// </summary>
        /// <param name="movie">The movie</param>
        private SubtitlesViewModel(MovieFull movie)
        {
            CancellationDownloadingSubtitlesToken = new CancellationTokenSource();
            if (SimpleIoc.Default.IsRegistered<MovieService>())
                MovieService = SimpleIoc.Default.GetInstance<MovieService>();

            Movie = movie;
        }

        /// <summary>
        /// Load asynchronously the movie's subtitles for the current instance
        /// </summary>
        /// <returns>Instance of SubtitlesViewModel</returns>
        private async Task<SubtitlesViewModel> InitializeAsync()
        {
            await LoadSubtitlesAsync(Movie);
            return this;
        }

        /// <summary>
        /// Initialize asynchronously an instance of the SubtitlesViewModel class
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <returns>Instance of SubtitlesViewModel</returns>
        public static Task<SubtitlesViewModel> CreateAsync(MovieFull movie)
        {
            var ret = new SubtitlesViewModel(movie);
            return ret.InitializeAsync();
        }

        /// <summary>
        /// Get the movie's subtitles
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <returns></returns>
        private async Task LoadSubtitlesAsync(MovieFull movie)
        {
            Logger.Debug(
                $"Load subtitles for movie: {movie.Title}");
            await MovieService.LoadSubtitlesAsync(movie, CancellationDownloadingSubtitlesToken.Token);
        }

        /// <summary>
        /// Stop downloading subtitles
        /// </summary>
        private void StopDownloadingSubtitles()
        {
            Logger.Debug(
                "Stop downloading subtitles");
            CancellationDownloadingSubtitlesToken.Cancel(true);
            CancellationDownloadingSubtitlesToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up SubtitlesViewModel");

            StopDownloadingSubtitles();
            base.Cleanup();
        }
    }
}