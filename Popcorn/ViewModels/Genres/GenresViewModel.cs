using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Genre;
using Popcorn.Services.Movie;
using TMDbLib.Objects.General;

namespace Popcorn.ViewModels.Genres
{
    public class GenresViewModel : ViewModelBase
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// Used to interact with movies
        /// </summary>
        private readonly MovieService _movieService;

        #region Properties

        #region Property -> CancellationLoadingGenres

        /// <summary>
        /// Used to cancel loading genres
        /// </summary>
        private CancellationTokenSource CancellationLoadingGenres { get; set; }

        #endregion

        #region Property -> MovieGenres

        private ObservableCollection<MovieGenre> _movieGenres = new ObservableCollection<MovieGenre>();

        /// <summary>
        /// Movie genres
        /// </summary>
        public ObservableCollection<MovieGenre> MovieGenres
        {
            get { return _movieGenres; }
            set { Set(() => MovieGenres, ref _movieGenres, value); }
        }

        #endregion

        #endregion

        private GenresViewModel()
        {
            RegisterMessages();
            CancellationLoadingGenres = new CancellationTokenSource();
            _movieService = SimpleIoc.Default.GetInstance<MovieService>();
        }

        #region Methods

        #region Method -> RegisterMessages

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                message =>
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                    {
                        StopLoadingGenres();
                        await LoadGenresAsync();
                    });
                });
        }

        #endregion

        #region Method -> InitializeAsync

        /// <summary>
        /// Load asynchronously the movie's genres for the current instance
        /// </summary>
        /// <returns>Instance of GenresViewModel</returns>
        private async Task<GenresViewModel> InitializeAsync()
        {
            Logger.Debug(
                "Initializing GenresViewModel");

            await LoadGenresAsync();
            return this;
        }

        #endregion

        #region Method -> CreateAsync

        /// <summary>
        /// Initialize asynchronously an instance of the GenresViewModel class
        /// </summary>
        /// <returns>Instance of GenresViewModel</returns>
        public static Task<GenresViewModel> CreateAsync()
        {
            var ret = new GenresViewModel();
            return ret.InitializeAsync();
        }

        #endregion

        #region Method -> LoadGenresAsync

        /// <summary>
        /// Load genres asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task LoadGenresAsync()
        {
            MovieGenres?.Clear();
            MovieGenres = null;

            MovieGenres =
                new ObservableCollection<MovieGenre>(await _movieService.GetGenresAsync(CancellationLoadingGenres.Token));
            if (CancellationLoadingGenres.IsCancellationRequested)
                return;

            MovieGenres?.Insert(0, new MovieGenre
            {
                TmdbGenre = new Genre
                {
                    Id = int.MaxValue,
                    Name = LocalizationProviderHelper.GetLocalizedValue<string>("AllLabel")
                },
                EnglishName = string.Empty
            });
        }

        #endregion

        #region Method -> StopLoadingGenres

        /// <summary>
        /// Cancel the loading of genres
        /// </summary>
        private void StopLoadingGenres()
        {
            Logger.Debug(
                "Stop loading genres.");

            CancellationLoadingGenres.Cancel(true);
            CancellationLoadingGenres = new CancellationTokenSource();
        }

        #endregion

        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning a GenresViewModel.");

            StopLoadingGenres();

            base.Cleanup();
        }

        #endregion
    }
}