using System;
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
    /// <summary>
    /// The popular movies tab
    /// </summary>
    public sealed class PopularTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PopularTabViewModel class.
        /// </summary>
        public PopularTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of PopularTabViewModel");

            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("PopularTitleTab");
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("PopularTitleTab"); });

            Messenger.Default.Register<ChangeSelectedGenreMessage>(
                this,
                async message =>
                {
                    StopLoadingMovies();
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
                StopLoadingMovies();
                await LoadMoviesAsync();
            });
        }

        #endregion

        #region Method -> LoadMoviesAsync

        /// <summary>
        /// Load next page
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            if (Page == LastPage)
                return;

            var watch = Stopwatch.StartNew();

            Logger.Info(
                $"Loading page {Page}...");

            Page++;
            IsLoadingMovies = true;
            try
            {
                var movieResults =
                    await MovieService.GetPopularMoviesAsync(Page,
                        MaxMoviesPerPage,
                        CancellationLoadingMovies.Token,
                        Genre);
                var movies = movieResults.Item1.ToList();
                MaxNumberOfMovies = movieResults.Item2;

                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                if (!movies.Any())
                    LastPage = Page;

                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;

                await MovieHistoryService.ComputeMovieHistoryAsync(movies, CancellationLoadingMovies);
                await MovieService.DownloadCoverImageAsync(movies, CancellationLoadingMovies);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded page {Page} in {elapsedMs} milliseconds.");
            }
            catch (Exception exception)
            {
                Page--;
                Logger.Info(
                    $"Error while loading page {Page}: {exception.Message}");
            }
            finally
            {
                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;
                if (!IsMovieFound)
                    Page = 0;
            }
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
            StopLoadingMovies();
            Genre = genre.TmdbGenre.Name == LocalizationProviderHelper.GetLocalizedValue<string>("AllLabel") ? null : genre;
            Page = 0;
            Movies.Clear();
            await LoadMoviesAsync();
        }

        #endregion

        #endregion
    }
}