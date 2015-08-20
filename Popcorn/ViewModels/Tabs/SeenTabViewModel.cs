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
    public class SeenTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SeenTabViewModel class.
        /// </summary>
        public SeenTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of SeenTabViewModel");

            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("SeenTitleTab");
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("SeenTitleTab"); });

            Messenger.Default.Register<ChangeHasBeenSeenMovieMessage>(
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
        /// Load seen movies
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            var watch = Stopwatch.StartNew();

            Logger.Info(
                "Loading seen movies...");

            IsLoadingMovies = true;
            var favoritesMovies = await MovieHistoryService.GetSeenMoviesAsync();
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
                $"Loaded seen movies in {elapsedMs} milliseconds.");
        }

        #endregion

        #endregion
    }
}