using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using NLog;
using Popcorn.Comparers;
using Popcorn.Models.Genre;
using Popcorn.Models.Movie.Short;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    /// <summary>
    /// The recent movies tab
    /// </summary>
    public sealed class RecentTabViewModel : TabsViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the RecentTabViewModel class.
        /// </summary>
        public RecentTabViewModel()
        {
            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("RecentTitleTab");
        }

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

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            ReloadMovies = new RelayCommand(async () =>
            {
                if (SimpleIoc.Default.IsRegistered<MainViewModel>())
                {
                    var mainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
                    mainViewModel.IsConnectionInError = false;
                }

                StopLoadingMovies();
                await LoadMoviesAsync();
            });
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

            try
            {
                IsLoadingMovies = true;

                var movies =
                    await MovieService.GetRecentMoviesAsync(Page,
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
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded page {Page} in {elapsedMs} milliseconds.");
            }
        }
    }
}