using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Movie;
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

            Messenger.Default.Register<ChangeSelectedGenreMessage>(
                this,
                async message =>
                {
                    await LoadByGenreAsync(message.Genre);
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
                "Loading movies...");

            IsLoadingMovies = true;
            var favoritesMovies = await MovieHistoryService.GetSeenMoviesAsync(Genre);
            var movies = favoritesMovies.ToList();
            Movies.Clear();
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }

            IsLoadingMovies = false;
            IsMovieFound = Movies.Any();
            CurrentNumberOfMovies = Movies.Count;
            MaxNumberOfMovies = movies.Count;
            await MovieService.DownloadCoverImageAsync(Movies);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Info(
                $"Loaded movies in {elapsedMs} milliseconds.");
        }

        #endregion

        #region Method -> LoadByGenreAsync

        /// <summary>
        /// Load movies for a genre
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        private async Task LoadByGenreAsync(MovieGenre genre)
        {
            Genre = genre.TmdbGenre.Name == LocalizationProviderHelper.GetLocalizedValue<string>("AllLabel") ? null : genre;
            await LoadMoviesAsync();
        }

        #endregion

        #endregion
    }
}