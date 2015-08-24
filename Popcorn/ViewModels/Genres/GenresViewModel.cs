using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Movie;
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

        private readonly MovieService _movieService;

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

        private GenresViewModel()
        {
            RegisterMessages();
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
            Logger.Info(
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

        private async Task LoadGenresAsync()
        {
            MovieGenres?.Clear();
            MovieGenres = null;

            MovieGenres = new ObservableCollection<MovieGenre>(await _movieService.GetGenresAsync());
            MovieGenres.Insert(0, new MovieGenre
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

        #endregion
    }
}
