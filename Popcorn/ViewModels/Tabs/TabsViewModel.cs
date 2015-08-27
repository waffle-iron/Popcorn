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
using GalaSoft.MvvmLight.Ioc;
using NLog;
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
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        #region Property -> MovieService

        /// <summary>
        /// Services used to interact with movies
        /// </summary>
        protected MovieService MovieService { get; }

        #endregion

        #region Property -> MovieHistoryService

        /// <summary>
        /// Services used to interacts with movie history
        /// </summary>
        protected MovieHistoryService MovieHistoryService { get; }

        #endregion

        #region Property -> Movies

        private ObservableCollection<MovieShort> _movies = new ObservableCollection<MovieShort>();

        /// <summary>
        /// Tab's movies
        /// </summary>
        public ObservableCollection<MovieShort> Movies
        {
            get { return _movies; }
            set { Set(() => Movies, ref _movies, value); }
        }

        #endregion

        #region Property -> Genre

        private static MovieGenre _genre;

        /// <summary>
        /// The current movie genre
        /// </summary>
        protected MovieGenre Genre
        {
            get { return _genre; }
            private set { Set(() => Genre, ref _genre, value, true); }
        }

        #endregion

        #region Property -> CurrentNumberOfMovies

        private int _currentNumberOfMovies;

        /// <summary>
        /// The current number of movies in the tab
        /// </summary>
        public int CurrentNumberOfMovies
        {
            get { return _currentNumberOfMovies; }
            set { Set(() => CurrentNumberOfMovies, ref _currentNumberOfMovies, value); }
        }

        #endregion

        #region Property -> MaxNumberOfMovies

        private int _maxNumberOfMovies;

        /// <summary>
        /// The maximum number of movies found
        /// </summary>
        public int MaxNumberOfMovies
        {
            get { return _maxNumberOfMovies; }
            set { Set(() => MaxNumberOfMovies, ref _maxNumberOfMovies, value); }
        }

        #endregion

        #region Property -> Page

        /// <summary>
        /// Current page number of loaded movies
        /// </summary>
        protected int Page { get; set; }

        #endregion

        #region Property -> MaxMoviesPerPage

        /// <summary>
        /// Maximum movies number to load per page request
        /// </summary>
        public int MaxMoviesPerPage { protected get; set; }

        #endregion

        #region Property -> CancellationLoadingMovies

        /// <summary>
        /// Token to cancel movie loading
        /// </summary>
        protected CancellationTokenSource CancellationLoadingMovies { get; private set; }

        #endregion

        #region Property -> TabName

        private string _tabName;

        /// <summary>
        /// The name of the tab shown in the interface
        /// </summary>
        public string TabName
        {
            get { return _tabName; }
            set { Set(() => TabName, ref _tabName, value); }
        }

        #endregion

        #region Property -> IsLoadingMovies

        private bool _isLoadingMovies;

        /// <summary>
        /// Specify if movies are loading
        /// </summary>
        public bool IsLoadingMovies
        {
            get { return _isLoadingMovies; }
            protected set { Set(() => IsLoadingMovies, ref _isLoadingMovies, value); }
        }

        #endregion

        #region Property -> NumberOfLoadedMovies

        private bool _isMoviesFound = true;

        /// <summary>
        /// Indicates if there's any movie found
        /// </summary>
        public bool IsMovieFound
        {
            get { return _isMoviesFound; }
            set { Set(() => IsMovieFound, ref _isMoviesFound, value); }
        }

        #endregion


        #region Property -> Rating

        private static double _rating;

        /// <summary>
        /// Movie rating filter
        /// </summary>
        public double Rating
        {
            get { return _rating; }
            set { Set(() => Rating, ref _rating, value, true); }
        }

        #endregion

        #endregion

        #region Commands

        #region Command -> ReloadMovies

        /// <summary>
        /// Command used to reload movies
        /// </summary>
        public RelayCommand ReloadMovies { get; set; }

        #endregion

        #region Command -> SetFavoriteMovieCommand

        /// <summary>
        /// Command used to set a movie as favorite
        /// </summary>
        public RelayCommand<MovieShort> SetFavoriteMovieCommand { get; private set; }

        #endregion

        #region Command -> ChangeMovieGenreCommand

        /// <summary>
        /// Command used to load movie's genres
        /// </summary>
        public RelayCommand<MovieGenre> ChangeMovieGenreCommand { get; set; }

        #endregion

        #endregion

        #region Constructors

        #region Constructor -> MoviesViewModel

        /// <summary>
        /// Initializes a new instance of the TabsViewModel class.
        /// </summary>
        protected TabsViewModel()
        {
            Logger.Debug("Initializing a new instance of TabsViewModel");

            RegisterMessages();
            RegisterCommands();
            MovieService = SimpleIoc.Default.GetInstance<MovieService>();
            MovieHistoryService = SimpleIoc.Default.GetInstance<MovieHistoryService>();
            MaxMoviesPerPage = Constants.MaxMoviesPerPage;
            CancellationLoadingMovies = new CancellationTokenSource();
        }

        #endregion

        #endregion

        #region Methods

        #region Method -> RegisterMessages

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
                            Logger.Debug(
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

        #endregion

        #region Method -> RegisterCommands

        /// <summary>
        /// Register commands
        /// </summary>
        /// <returns></returns>
        private void RegisterCommands()
        {
            SetFavoriteMovieCommand =
                new RelayCommand<MovieShort>(async movie =>
                {
                    await MovieHistoryService.SetFavoriteMovieAsync(movie, CancellationLoadingMovies);
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

        #endregion

        #region Method -> LoadMoviesAsync

        /// <summary>
        /// Load movies
        /// </summary>
        public virtual Task LoadMoviesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Method -> StopLoadingMovies

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

        #endregion

        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning a TabViewModel.");

            StopLoadingMovies();

            base.Cleanup();
        }

        #endregion
    }
}