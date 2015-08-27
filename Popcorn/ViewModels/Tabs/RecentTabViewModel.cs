using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.Models.Movie;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// The recent movies tab
    /// </summary>
    public sealed class RecentTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the RecentTabViewModel class.
        /// </summary>
        public RecentTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of RecentTabViewModel");

            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("RecentTitleTab");
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("RecentTitleTab"); });

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
        /// Load next page
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            try
            {
                if (Page == LastPage)
                    return;

                var watch = Stopwatch.StartNew();

                Logger.Info(
                    $"Loading page {Page}...");

                Page++;
                IsLoadingMovies = true;

                var movieResults =
                    await MovieService.GetRecentMoviesAsync(Page,
                        MaxMoviesPerPage,
                        Rating,
                        CancellationLoadingMovies.Token,
                        Genre);

                var movies = movieResults.Item1.ToList();
                MaxNumberOfMovies = movieResults.Item2;

                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                if (!movies.Any() && MaxNumberOfMovies != 0)
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

        #endregion
    }
}