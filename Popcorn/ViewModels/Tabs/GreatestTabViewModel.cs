using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// The greatest movies tab
    /// </summary>
    public sealed class GreatestTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GreatestTabViewModel class.
        /// </summary>
        public GreatestTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of GreatestTitleTab");

            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("GreatestTitleTab");
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("GreatestTitleTab"); });
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
        /// Load next page with an optional search parameter
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            if (Page == LastPage)
                return;

            var watch = Stopwatch.StartNew();

            Logger.Info(
                $"Loading page {Page} of the greatest movies...");

            Page++;
            IsLoadingMovies = true;
            try
            {
                var movieResults =
                    await MovieService.GetGreatestMoviesAsync(Page,
                        MaxMoviesPerPage,
                        CancellationLoadNextPageToken.Token);
                var movies = movieResults.Item1.ToList();
                MaxNumberOfMovies = movieResults.Item2;

                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                if (!movies.Any())
                    LastPage = Page;

                IsLoadingMovies = false;
                CurrentNumberOfMovies = Movies.Count();

                await MovieHistoryService.ComputeMovieHistoryAsync(movies);
                await MovieService.DownloadCoverImageAsync(movies);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded page {Page} of the greatest movies in {elapsedMs} milliseconds.");
            }
            catch (Exception exception)
            {
                Page--;

                Logger.Info(
                    $"Error while loading page {Page} of the greatest movies : {exception.Message}");
            }
            finally
            {
                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count();
                if (!IsMovieFound)
                    Page = 0;
            }
        }

        #endregion

        #endregion
    }
}