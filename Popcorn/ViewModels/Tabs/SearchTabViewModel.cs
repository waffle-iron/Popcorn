using System;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using Popcorn.Helpers;
using Popcorn.Messaging;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.Models.Genre;
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
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("SearchTitleTab");
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

            Messenger.Default.Register<PropertyChangedMessage<MovieGenre>>(this, async e =>
            {
                if (e.PropertyName != GetPropertyName(() => Genre) && Genre.Equals(e.NewValue)) return;
                StopLoadingMovies();
                Page = 0;
                Movies.Clear();
                await SearchMoviesAsync(SearchFilter);
            });

            Messenger.Default.Register<PropertyChangedMessage<double>>(this, async e =>
            {
                if (e.PropertyName != GetPropertyName(() => Rating) && Rating.Equals(e.NewValue)) return;
                StopLoadingMovies();
                Page = 0;
                Movies.Clear();
                await SearchMoviesAsync(SearchFilter);
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
                await SearchMoviesAsync(SearchFilter);
            });
        }

        #endregion

        #region Method -> SearchMoviesAsync

        /// <summary>
        /// Search movies
        /// </summary>
        /// <param name="searchFilter">The parameter of the search</param>
        public async Task SearchMoviesAsync(string searchFilter)
        {
            if (SearchFilter != searchFilter)
            {
                // We start an other search
                StopLoadingMovies();
                Movies.Clear();
                Page = 0;
            }

            var watch = Stopwatch.StartNew();

            Page++;

            Logger.Info(
                $"Loading page {Page} with criteria: {searchFilter}");

            try
            {
                SearchFilter = searchFilter;

                IsLoadingMovies = true;

                var movieResults =
                    await MovieService.SearchMoviesAsync(searchFilter,
                        Page,
                        MaxMoviesPerPage,
                        Genre,
                        Rating,
                        CancellationLoadingMovies.Token);

                var movies = movieResults.Item1.ToList();

                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;
                MaxNumberOfMovies = movieResults.Item2;

                await MovieHistoryService.ComputeMovieHistoryAsync(movies);
                await MovieService.DownloadCoverImageAsync(movies, CancellationLoadingMovies);
            
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded page {Page} with criteria {searchFilter} in {elapsedMs} milliseconds.");
            }
            catch (Exception exception)
            {
                Page--;
                Logger.Error(
                    $"Error while loading page {Page} with criteria {searchFilter}: {exception.Message}");
            }
            finally
            {
                watch.Stop();
            }
        }

        #endregion

        #endregion
    }
}