using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Comparers;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Models.Genre;
using Popcorn.Models.Movie.Short;
using Popcorn.Services.History;
using Popcorn.Services.Movie;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// The popular movies tab
    /// </summary>
    public sealed class PopularTabViewModel : TabsViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the PopularTabViewModel class.
        /// </summary>
        /// <param name="applicationState">Application state</param>
        /// <param name="movieService">Movie service</param>
        /// <param name="movieHistoryService">Movie history service</param>
        public PopularTabViewModel(IApplicationState applicationState, IMovieService movieService, IMovieHistoryService movieHistoryService)
            : base(applicationState, movieService, movieHistoryService)
        {
            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("PopularTitleTab");
        }

        /// <summary>
        /// Load next page
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            var watch = Stopwatch.StartNew();

            Page++;

            Logger.Info(
                $"Loading page {Page}...");

            HasLoadingFailed = false;

            try
            {
                IsLoadingMovies = true;

                var movies =
                    await MovieService.GetPopularMoviesAsync(Page,
                        MaxMoviesPerPage,
                        Rating,
                        CancellationLoadingMovies.Token,
                        Genre);

                Movies = new ObservableCollection<MovieShort>(Movies.Union(movies.Item1, new MovieShortComparer()));

                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;
                MaxNumberOfMovies = movies.Item2;

                await MovieHistoryService.ComputeMovieHistoryAsync(movies.Item1);
                await MovieService.DownloadCoverImageAsync(movies.Item1, CancellationLoadingMovies);
            }
            catch (Exception exception)
            {
                Page--;
                Logger.Error(
                    $"Error while loading page {Page}: {exception.Message}");
                HasLoadingFailed = true;
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded page {Page} in {elapsedMs} milliseconds.");
            }
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("PopularTitleTab"); });

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

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            ReloadMovies = new RelayCommand(async () =>
            {
                ApplicationState.IsConnectionInError = false;
                StopLoadingMovies();
                await LoadMoviesAsync();
            });
        }
    }
}