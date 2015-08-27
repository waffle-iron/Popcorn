using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
                    StopLoadingMovies();
                    await LoadMoviesAsync();
                });

            Messenger.Default.Register<PropertyChangedMessage<MovieGenre>>(this, async e =>
            {
                if (e.PropertyName != GetPropertyName(() => Genre) && Genre.Equals(e.NewValue)) return;
                StopLoadingMovies();
                Page = 0;
                Movies.Clear();
                await LoadMoviesAsync();
            });

            Messenger.Default.Register<PropertyChangedMessage<double>>(this, async e =>
            {
                if (e.PropertyName != GetPropertyName(() => Rating) && Rating.Equals(e.NewValue)) return;
                StopLoadingMovies();
                Page = 0;
                Movies.Clear();
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
                StopLoadingMovies();
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
            try
            {
                if (Page == LastPage)
                    return;

                var watch = Stopwatch.StartNew();

                Logger.Info(
                    "Loading movies...");

                IsLoadingMovies = true;
                var favoritesMovies =
                    await MovieHistoryService.GetSeenMoviesAsync(Genre, Rating, CancellationLoadingMovies);

                var movies = favoritesMovies.ToList();
                Movies.Clear();
                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                if (!movies.Any() && MaxNumberOfMovies != 0)
                    LastPage = Page;

                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;
                MaxNumberOfMovies = movies.Count;
                await MovieService.DownloadCoverImageAsync(Movies, CancellationLoadingMovies);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded movies in {elapsedMs} milliseconds.");
            }
            catch (Exception exception)
            {
                Logger.Info(
                    $"Error while loading page {Page}: {exception.Message}");
            }
        }

        #endregion

        #endregion
    }
}