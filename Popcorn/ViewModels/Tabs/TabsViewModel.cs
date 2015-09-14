using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using Popcorn.Helpers;
using Popcorn.Messaging;
using NLog;
using Popcorn.Models.ApplicationState;
using Popcorn.Models.Genre;
using Popcorn.Models.Movie.Short;
using Popcorn.Services.History;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// Manage tab controls
    /// </summary>
    public class TabsViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Services used to interact with movies
        /// </summary>
        protected readonly IMovieService MovieService;

        /// <summary>
        /// Services used to interact with movie history
        /// </summary>
        protected readonly IMovieHistoryService MovieHistoryService;

        private static MovieGenre _genre;

        private static double _rating;

        private ObservableCollection<MovieShort> _movies = new ObservableCollection<MovieShort>();

        private int _currentNumberOfMovies;

        private int _maxNumberOfMovies;

        private string _tabName;

        private bool _isLoadingMovies;

        private bool _isMoviesFound = true;

        private bool _hasLoadingFailed;

        /// <summary>
        /// Initializes a new instance of the TabsViewModel class.
        /// </summary>
        protected TabsViewModel(IApplicationState applicationState, IMovieService movieService, IMovieHistoryService movieHistoryService)
        {
            ApplicationState = applicationState;
            MovieService = movieService;
            MovieHistoryService = movieHistoryService;

            RegisterMessages();
            RegisterCommands();

            MaxMoviesPerPage = Constants.MaxMoviesPerPage;
            CancellationLoadingMovies = new CancellationTokenSource();
        }

        /// <summary>
        /// Services used to interact with movie history
        /// </summary>
        public IApplicationState ApplicationState { get; set; }

        /// <summary>
        /// Tab's movies
        /// </summary>
        public ObservableCollection<MovieShort> Movies
        {
            get { return _movies; }
            set { Set(() => Movies, ref _movies, value); }
        }

        /// <summary>
        /// The current number of movies in the tab
        /// </summary>
        public int CurrentNumberOfMovies
        {
            get { return _currentNumberOfMovies; }
            set { Set(() => CurrentNumberOfMovies, ref _currentNumberOfMovies, value); }
        }

        /// <summary>
        /// The maximum number of movies found
        /// </summary>
        public int MaxNumberOfMovies
        {
            get { return _maxNumberOfMovies; }
            set { Set(() => MaxNumberOfMovies, ref _maxNumberOfMovies, value); }
        }

        /// <summary>
        /// The name of the tab shown in the interface
        /// </summary>
        public string TabName
        {
            get { return _tabName; }
            set { Set(() => TabName, ref _tabName, value); }
        }

        /// <summary>
        /// Specify if movies are loading
        /// </summary>
        public bool IsLoadingMovies
        {
            get { return _isLoadingMovies; }
            protected set { Set(() => IsLoadingMovies, ref _isLoadingMovies, value); }
        }

        /// <summary>
        /// Indicates if there's any movie found
        /// </summary>
        public bool IsMovieFound
        {
            get { return _isMoviesFound; }
            set { Set(() => IsMovieFound, ref _isMoviesFound, value); }
        }

        /// <summary>
        /// Movie rating filter
        /// </summary>
        public double Rating
        {
            get { return _rating; }
            set { Set(() => Rating, ref _rating, value, true); }
        }

        /// <summary>
        /// Command used to reload movies
        /// </summary>
        public RelayCommand ReloadMovies { get; set; }

        /// <summary>
        /// Command used to set a movie as favorite
        /// </summary>
        public RelayCommand<MovieShort> SetFavoriteMovieCommand { get; private set; }

        /// <summary>
        /// Command used to load movie's genres
        /// </summary>
        public RelayCommand<MovieGenre> ChangeMovieGenreCommand { get; set; }

        public bool HasLoadingFailed
        {
            get { return _hasLoadingFailed; }
            set { Set(() => HasLoadingFailed, ref _hasLoadingFailed, value); }
        }

        /// <summary>
        /// The current movie genre
        /// </summary>
        protected MovieGenre Genre
        {
            get { return _genre; }
            private set { Set(() => Genre, ref _genre, value, true); }
        }

        /// <summary>
        /// Current page number of loaded movies
        /// </summary>
        protected int Page { get; set; }

        /// <summary>
        /// Maximum movies number to load per page request
        /// </summary>
        protected int MaxMoviesPerPage { get; }

        /// <summary>
        /// Token to cancel movie loading
        /// </summary>
        protected CancellationTokenSource CancellationLoadingMovies { get; private set; }

        /// <summary>
        /// Load movies
        /// </summary>
        public virtual Task LoadMoviesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            StopLoadingMovies();
            base.Cleanup();
        }

        /// <summary>
        /// Cancel the loading of the next page 
        /// </summary>
        protected void StopLoadingMovies()
        {
            Logger.Info(
                "Stop loading movies.");

            CancellationLoadingMovies.Cancel(true);
            CancellationLoadingMovies = new CancellationTokenSource();
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                async message =>
                {
                    foreach (var movie in Movies.ToList())
                    {
                        try
                        {
                            await Task.Delay(1000, CancellationLoadingMovies.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            Logger.Info(
                                $"Stopped translating movie : {movie.Title}");
                            return;
                        }

                        await MovieService.TranslateMovieShortAsync(movie, CancellationLoadingMovies.Token);
                    }
                });

            Messenger.Default.Register<ChangeFavoriteMovieMessage>(
                this,
                async message =>
                {
                    await MovieHistoryService.ComputeMovieHistoryAsync(Movies);
                });
        }

        /// <summary>
        /// Register commands
        /// </summary>
        /// <returns></returns>
        private void RegisterCommands()
        {
            SetFavoriteMovieCommand =
                new RelayCommand<MovieShort>(async movie =>
                {
                    await MovieHistoryService.SetFavoriteMovieAsync(movie);
                    Messenger.Default.Send(new ChangeFavoriteMovieMessage());
                });

            ChangeMovieGenreCommand =
                new RelayCommand<MovieGenre>(genre =>
                {
                    Genre = genre.TmdbGenre.Name ==
                            LocalizationProviderHelper.GetLocalizedValue<string>("AllLabel")
                        ? null
                        : genre;
                });
        }
    }
}