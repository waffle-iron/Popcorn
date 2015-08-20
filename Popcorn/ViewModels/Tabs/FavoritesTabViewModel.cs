using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Comparers;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    public class FavoritesTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FavoritesTabViewModel class.
        /// </summary>
        public FavoritesTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of FavoritesTabViewModel");
            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("FavoritesTitleTab");
        }

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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("FavoritesTitleTab"); });

            Messenger.Default.Register<ChangeFavoriteMovieMessage>(
                this,
                async message =>
                {
                    await LoadMoviesAsync();
                });
        }

        #endregion

        #region Method -> RegisterCommands

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            ReloadMovies = new RelayCommand(async () =>
            {
                var mainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
                mainViewModel.IsConnectionInError = false;
                await LoadMoviesAsync();
            });
        }

        #endregion

        #region Method -> LoadMoviesAsync

        /// <summary>
        /// Load favorites movies
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            var watch = Stopwatch.StartNew();

            Logger.Info(
                "Loading favorites movies...");

            IsLoadingMovies = true;
            var favoritesMovies = await MovieHistoryService.GetFavoritesMoviesAsync();
            var movies = favoritesMovies.ToList();
            var moviesToAdd = movies.Except(Movies, new MovieShortComparer()).ToList();
            var moviesToRemove = Movies.Except(movies, new MovieShortComparer()).ToList();
            foreach (var movie in moviesToAdd)
            {
                Movies.Add(movie);
            }

            foreach (var movie in moviesToRemove)
            {
                Movies.Remove(movie);
            }

            IsLoadingMovies = false;
            IsMovieFound = Movies.Any();
            CurrentNumberOfMovies = Movies.Count();
            MaxNumberOfMovies = movies.Count();
            await MovieService.DownloadCoverImageAsync(moviesToAdd);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Info(
                $"Loaded favorites movies in {elapsedMs} milliseconds.");
        }

        #endregion

        #endregion
    }
}