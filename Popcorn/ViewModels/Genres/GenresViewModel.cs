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
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used to interact with movies
        /// </summary>
        private readonly MovieService _movieService;

        /// <summary>
        /// Used to cancel loading genres
        /// </summary>
        private CancellationTokenSource CancellationLoadingGenres { get; set; }

        private ObservableCollection<MovieGenre> _movieGenres = new ObservableCollection<MovieGenre>();

        /// <summary>
        /// Movie genres
        /// </summary>
        public ObservableCollection<MovieGenre> MovieGenres
        {
            get { return _movieGenres; }
            set { Set(() => MovieGenres, ref _movieGenres, value); }
        }

        /// <summary>
        /// Initialize a new instance of GenresViewModel class
        /// </summary>
        private GenresViewModel()
        {
            RegisterMessages();
            CancellationLoadingGenres = new CancellationTokenSource();
            if(SimpleIoc.Default.IsRegistered<MovieService>())
               _movieService = SimpleIoc.Default.GetInstance<MovieService>();
        }

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

        /// <summary>
        /// Load asynchronously the movie's genres for the current instance
        /// </summary>
        /// <returns>Instance of GenresViewModel</returns>
        private async Task<GenresViewModel> InitializeAsync()
        {
            await LoadGenresAsync();
            return this;
        }

        /// <summary>
        /// Initialize asynchronously an instance of the GenresViewModel class
        /// </summary>
        /// <returns>Instance of GenresViewModel</returns>
        public static Task<GenresViewModel> CreateAsync()
        {
            var ret = new GenresViewModel();
            return ret.InitializeAsync();
        }

        /// <summary>
        /// Load genres asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task LoadGenresAsync()
        {
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

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning a GenresViewModel.");

            StopLoadingGenres();

            base.Cleanup();
        }
    }
}