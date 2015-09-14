using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Messaging;
using Popcorn.Models.Movie.Full;
using Popcorn.Models.Movie.Short;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.DownloadMovie;
using Popcorn.ViewModels.Trailer;

namespace Popcorn.ViewModels.Movie
{
    /// <summary>
    /// Manage the movie
    /// </summary>
    public sealed class MovieViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The service used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        private MovieFull _movie = new MovieFull();

        private IDownloadMovieViewModel _downloadMovie;

        private ITrailerViewModel _trailer;

        private bool _isMovieLoading;

        private bool _isTrailerLoading;

        private bool _isPlayingTrailer;

        private bool _isDownloadingMovie;

        /// <summary>
        /// Token to cancel trailer loading
        /// </summary>
        private CancellationTokenSource _cancellationLoadingTrailerToken;

        /// <summary>
        /// Token to cancel movie loading
        /// </summary>
        private CancellationTokenSource _cancellationLoadingToken;

        /// <summary>
        /// Initializes a new instance of the MovieViewModel class.
        /// </summary>
        /// <param name="downloadMovieViewModel">ViewModel which manages the movie download</param>
        /// <param name="movieService">Service used to interact with movies</param>
        /// <param name="trailerViewModel">ViewModel which manages the trailer</param>
        public MovieViewModel(IDownloadMovieViewModel downloadMovieViewModel, IMovieService movieService,
            ITrailerViewModel trailerViewModel)
        {
            _movieService = movieService;
            _cancellationLoadingToken = new CancellationTokenSource();
            _cancellationLoadingTrailerToken = new CancellationTokenSource();
            DownloadMovie = downloadMovieViewModel;
            Trailer = trailerViewModel;
            RegisterMessages();
            RegisterCommands();
        }

        /// <summary>
        /// The selected movie to show into the interface
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// Indicates if a movie is loading
        /// </summary>
        public bool IsMovieLoading
        {
            get { return _isMovieLoading; }
            set { Set(() => IsMovieLoading, ref _isMovieLoading, value); }
        }

        /// <summary>
        /// View model which takes care of downloading the movie
        /// </summary>
        public IDownloadMovieViewModel DownloadMovie
        {
            get { return _downloadMovie; }
            set { Set(() => DownloadMovie, ref _downloadMovie, value); }
        }

        /// <summary>
        /// View model which takes care of the movie's trailer
        /// </summary>

        public ITrailerViewModel Trailer
        {
            get { return _trailer; }
            set { Set(() => Trailer, ref _trailer, value); }
        }

        /// <summary>
        /// Specify if a trailer is loading
        /// </summary>
        public bool IsTrailerLoading
        {
            get { return _isTrailerLoading; }
            set { Set(() => IsTrailerLoading, ref _isTrailerLoading, value); }
        }

        /// <summary>
        /// Specify if a trailer is loading
        /// </summary>
        public bool IsPlayingTrailer
        {
            get { return _isPlayingTrailer; }
            set { Set(() => IsPlayingTrailer, ref _isPlayingTrailer, value); }
        }

        /// <summary>
        /// Specify if a movie is downloading
        /// </summary>
        public bool IsDownloadingMovie
        {
            get { return _isDownloadingMovie; }
            set { Set(() => IsDownloadingMovie, ref _isDownloadingMovie, value); }
        }

        /// <summary>
        /// Command used to load the movie
        /// </summary>
        public RelayCommand<MovieShort> LoadMovieCommand { get; private set; }

        /// <summary>
        /// Stop loading the trailer
        /// </summary>
        public RelayCommand StopLoadingTrailerCommand { get; private set; }

        /// <summary>
        /// Command used to play the movie
        /// </summary>
        public RelayCommand PlayMovieCommand { get; private set; }

        /// <summary>
        /// Command used to play the trailer
        /// </summary>
        public RelayCommand PlayTrailerCommand { get; private set; }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            StopLoadingMovie();
            StopPlayingMovie();
            StopLoadingTrailer();
            StopPlayingTrailer();
            base.Cleanup();
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<StopPlayingTrailerMessage>(
                this,
                message => { StopPlayingTrailer(); });

            Messenger.Default.Register<StopPlayingMovieMessage>(
                this,
                message => { StopPlayingMovie(); });

            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                async message =>
                {
                    if (!string.IsNullOrEmpty(Movie?.ImdbCode))
                    {
                        await _movieService.TranslateMovieFullAsync(Movie, _cancellationLoadingToken.Token);
                    }
                });
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            LoadMovieCommand = new RelayCommand<MovieShort>(async movie => { await LoadMovieAsync(movie); });

            PlayMovieCommand = new RelayCommand(() =>
            {
                IsDownloadingMovie = true;
                DownloadMovie.LoadMovie(Movie);
            });

            PlayTrailerCommand = new RelayCommand(async () =>
            {
                IsPlayingTrailer = true;
                IsTrailerLoading = true;
                await Trailer.LoadTrailerAsync(Movie, _cancellationLoadingTrailerToken.Token);
                IsTrailerLoading = false;
            });

            StopLoadingTrailerCommand = new RelayCommand(StopLoadingTrailer);
        }

        /// <summary>
        /// Get the requested movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        private async Task LoadMovieAsync(MovieShort movie)
        {
            var watch = Stopwatch.StartNew();

            Messenger.Default.Send(new LoadMovieMessage());
            IsMovieLoading = true;

            Movie = await _movieService.GetMovieFullDetailsAsync(movie, _cancellationLoadingToken.Token);
            IsMovieLoading = false;
            await _movieService.DownloadPosterImageAsync(Movie, _cancellationLoadingToken);
            await _movieService.DownloadDirectorImageAsync(Movie, _cancellationLoadingToken);
            await _movieService.DownloadActorImageAsync(Movie, _cancellationLoadingToken);
            await _movieService.DownloadBackgroundImageAsync(Movie, _cancellationLoadingToken);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug($"LoadMovieAsync ({movie.ImdbCode}) in {elapsedMs} milliseconds.");

        }

        /// <summary>
        /// Stop loading the movie
        /// </summary>
        private void StopLoadingMovie()
        {
            Logger.Info(
                $"Stop loading movie: {Movie.Title}.");

            IsMovieLoading = false;
            _cancellationLoadingToken.Cancel(true);
            _cancellationLoadingToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Stop playing a trailer
        /// </summary>
        private void StopLoadingTrailer()
        {
            Logger.Info(
                $"Stop loading movie's trailer: {Movie.Title}.");

            IsTrailerLoading = false;
            _cancellationLoadingTrailerToken.Cancel(true);
            _cancellationLoadingTrailerToken = new CancellationTokenSource();
            StopPlayingTrailer();
        }

        /// <summary>
        /// Stop playing a trailer
        /// </summary>
        private void StopPlayingTrailer()
        {
            Logger.Info(
                $"Stop playing movie's trailer: {Movie.Title}.");

            IsPlayingTrailer = false;
            Trailer.UnLoadTrailer();
        }

        /// <summary>
        /// Stop playing a movie
        /// </summary>
        private void StopPlayingMovie()
        {
            Logger.Info(
                $"Stop playing movie: {Movie.Title}.");

            IsDownloadingMovie = false;
            DownloadMovie.StopDownloadingMovie();
        }
    }
}