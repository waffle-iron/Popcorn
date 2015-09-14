using GalaSoft.MvvmLight;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Models.Movie.Full;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Subtitles
{
    public sealed class SubtitlesViewModel : ViewModelBase, ISubtitlesViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The service used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        private MovieFull _movie;

        /// <summary>
        /// Token to cancel downloading subtitles
        /// </summary>
        private CancellationTokenSource _cancellationDownloadingSubtitlesToken;

        /// <summary>
        /// Initializes a new instance of the SubtitlesViewModel class.
        /// </summary>
        /// <param name="movieService">The movie service</param>
        public SubtitlesViewModel(IMovieService movieService)
        {
            _movieService = movieService;
            _cancellationDownloadingSubtitlesToken = new CancellationTokenSource();
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
        /// Get the movie's subtitles
        /// </summary>
        /// <param name="movie">The movie</param>
        public async Task LoadSubtitlesAsync(MovieFull movie)
        {
            Logger.Debug(
                $"Load subtitles for movie: {movie.Title}");
            Movie = movie;
            await _movieService.LoadSubtitlesAsync(movie, _cancellationDownloadingSubtitlesToken.Token);
        }

        /// <summary>
        /// Stop downloading subtitles and clear movie
        /// </summary>
        public void ClearSubtitles()
        {
            StopDownloadingSubtitles();
            Movie = null;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            ClearSubtitles();
            base.Cleanup();
        }

        /// <summary>
        /// Stop downloading subtitles
        /// </summary>
        private void StopDownloadingSubtitles()
        {
            Logger.Debug(
                "Stop downloading subtitles");
            _cancellationDownloadingSubtitlesToken.Cancel(true);
            _cancellationDownloadingSubtitlesToken = new CancellationTokenSource();
        }
    }
}