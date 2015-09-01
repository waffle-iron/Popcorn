using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Genre;
using Popcorn.Models.Movie.Short;
using Popcorn.ViewModels.Main;

namespace Popcorn.ViewModels.Tabs
{
    public class FavoritesTabViewModel : TabsViewModel
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FavoritesTabViewModel class.
        /// </summary>
        public FavoritesTabViewModel()
        {
            Logger.Debug(
                "Initializing a new instance of FavoritesTabViewModel");
            RegisterMessages();
            RegisterCommands();
            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("FavoritesTitleTab");
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
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("FavoritesTitleTab"); });

            Messenger.Default.Register<ChangeFavoriteMovieMessage>(
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
                await LoadMoviesAsync();
            });

            Messenger.Default.Register<PropertyChangedMessage<double>>(this, async e =>
            {
                if (e.PropertyName != GetPropertyName(() => Rating) && Rating.Equals(e.NewValue)) return;
                StopLoadingMovies();
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
                if (SimpleIoc.Default.IsRegistered<MainViewModel>())
                {
                    var mainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
                    mainViewModel.IsConnectionInError = false;
                }

                StopLoadingMovies();
                await LoadMoviesAsync();
            });
        }

        #endregion

        #region Method -> LoadMoviesAsync

        /// <summary>
        /// Load movies
        /// </summary>
        public override async Task LoadMoviesAsync()
        {
            var watch = Stopwatch.StartNew();

            Logger.Info(
                "Loading movies...");

            try
            {
                IsLoadingMovies = true;

                var movies =
                    await
                        MovieHistoryService.GetFavoritesMoviesAsync(Genre, Rating);

                Movies = new ObservableCollection<MovieShort>(movies);

                IsLoadingMovies = false;
                IsMovieFound = Movies.Any();
                CurrentNumberOfMovies = Movies.Count;
                MaxNumberOfMovies = Movies.Count;

                await MovieService.DownloadCoverImageAsync(Movies, CancellationLoadingMovies);
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"Error while loading page {Page}: {exception.Message}");
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Info(
                    $"Loaded movies in {elapsedMs} milliseconds.");
            }
        }

        #endregion

        #endregion
    }
}