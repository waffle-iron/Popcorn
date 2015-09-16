using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
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

        /// <summary>
        /// Token to cancel downloading subtitles
        /// </summary>
        private CancellationTokenSource _cancellationDownloadingSubtitlesToken;

        /// <summary>
        /// Specify if movie's subtitles are enabled
        /// </summary>
        private bool _enabledSubtitles;

        /// <summary>
        /// The movie to manage
        /// </summary>
        private MovieFull _movie;

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
        /// The movie to manage
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// Specify if movie's subtitles are enabled
        /// </summary>
        public bool EnabledSubtitles
        {
            get { return _enabledSubtitles; }
            set { Set(() => EnabledSubtitles, ref _enabledSubtitles, value); }
        }

        /// <summary>
        /// Load the movie's subtitles asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        public async Task LoadSubtitlesAsync(MovieFull movie)
        {
            Logger.Debug(
                $"Load subtitles for movie: {movie.Title}");
            Movie = movie;
            EnabledSubtitles = true;
            await _movieService.LoadSubtitlesAsync(movie, _cancellationDownloadingSubtitlesToken.Token);
        }

        /// <summary>
        /// Stop downloading subtitles and clear movie
        /// </summary>
        public void ClearSubtitles()
        {
            EnabledSubtitles = false;
            StopDownloadingSubtitles();
            if (Movie != null)
                Movie.SelectedSubtitle = null;
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