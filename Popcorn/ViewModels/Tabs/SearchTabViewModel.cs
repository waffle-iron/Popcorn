using System;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using Popcorn.Helpers;
using Popcorn.Messaging;
using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.Models.Movie;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// The search movies tab
    /// </summary>
    public sealed class SearchTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property -> CancellationSearchMoviesToken

        /// <summary>
        /// Token to cancel the search
        /// </summary>
        private CancellationTokenSource CancellationSearchToken { get; set; }

        #endregion

        #region Property -> LastPageFilterMapping

        /// <summary>
        /// Used to determine the last page of the searched criteria
        /// </summary>
        private Dictionary<string, int> LastPageFilterMapping { get; }

        #endregion

        #region Property -> SearchFilter

        /// <summary>
        /// The search filter
        /// </summary>
        public string SearchFilter { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SearchTabViewModel class.
        /// </summary>
        public SearchTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of SearchTabViewModel");

            RegisterMessages();
            RegisterCommands();
            CancellationSearchToken = new CancellationTokenSource();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("SearchTitleTab");
            LastPageFilterMapping = new Dictionary<string, int>();
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("SearchTitleTab"); });

            Messenger.Default.Register<ChangeSelectedGenreMessage>(
                this,
                async message =>
                {
                    StopSearchingMovies();
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
                StopSearchingMovies();
                await SearchMoviesAsync(SearchFilter);
            });
        }

        #endregion

        #region Method -> SearchMoviesAsync

        /// <summary>
        /// Search movies
        /// </summary>
        /// <param name="searchFilter">The parameter of the search</param>
        /// <param name="ct">Used to cancel the task</param>
        public async Task SearchMoviesAsync(string searchFilter)
        {
            if (SearchFilter != searchFilter)
            {
                // We start an other search
                StopSearchingMovies();
                Movies.Clear();
                Page = 0;
            }

            var watch = Stopwatch.StartNew();

            Logger.Info(
                $"Loading page {Page} with criteria: {searchFilter}");

            SearchFilter = searchFilter;
            Page++;
            int lastPage;
            if (!LastPageFilterMapping.ContainsKey(searchFilter) ||
                (LastPageFilterMapping.TryGetValue(searchFilter, out lastPage) && Page < lastPage))
            {
                try
                {
                    IsLoadingMovies = true;

                    var movieResults =
                        await MovieService.SearchMoviesAsync(searchFilter,
                            Page,
                            MaxMoviesPerPage,
                            CancellationSearchToken.Token,
                            Genre);
                    var movies = movieResults.Item1.ToList();
                    MaxNumberOfMovies = movieResults.Item2;

                    foreach (var movie in movies)
                    {
                        Movies.Add(movie);
                    }

                    IsLoadingMovies = false;
                    IsMovieFound = Movies.Any();
                    CurrentNumberOfMovies = Movies.Count;

                    await MovieHistoryService.ComputeMovieHistoryAsync(movies, CancellationLoadingMovies);
                    await MovieService.DownloadCoverImageAsync(movies, CancellationLoadingMovies);
                    if (!LastPageFilterMapping.ContainsKey(searchFilter) && !movies.Any())
                    {
                        LastPageFilterMapping.Add(searchFilter, Page);
                    }


                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Logger.Info(
                        $"Loaded page {Page} with criteria {searchFilter} in {elapsedMs} milliseconds.");
                }
                catch (Exception exception)
                {
                    Page--;
                    Logger.Info(
                        $"Error while loading page {Page} with criteria {searchFilter}: {exception.Message}");
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
            StopSearchingMovies();
            Genre = genre.TmdbGenre.Name == LocalizationProviderHelper.GetLocalizedValue<string>("AllLabel") ? null : genre;
            Page = 0;
            Movies.Clear();
            await LoadMoviesAsync();
        }

        #endregion

        #region Method -> StopSearchingMovies

        /// <summary>
        /// Cancel searching movies
        /// </summary>
        private void StopSearchingMovies()
        {
            Logger.Info(
                "Stop searching movies.");

            CancellationSearchToken?.Cancel();
            CancellationSearchToken = new CancellationTokenSource();
        }

        #endregion

        #endregion

        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up SearchTabViewModel.");

            StopSearchingMovies();
            CancellationSearchToken?.Dispose();

            base.Cleanup();
        }
    }
}