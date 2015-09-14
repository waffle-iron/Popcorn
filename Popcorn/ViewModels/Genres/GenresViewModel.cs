using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
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
    public class GenresViewModel : ViewModelBase, IGenresViewModel
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        private ObservableCollection<MovieGenre> _movieGenres = new ObservableCollection<MovieGenre>();

        /// <summary>
        /// Used to cancel loading genres
        /// </summary>
        private CancellationTokenSource _cancellationLoadingGenres;

        /// <summary>
        /// Initialize a new instance of GenresViewModel class
        /// </summary>
        /// <param name="movieService">The movie service</param>
        public GenresViewModel(IMovieService movieService)
        {
            _movieService = movieService;
            _cancellationLoadingGenres = new CancellationTokenSource();
            RegisterMessages();
        }

        /// <summary>
        /// Movie genres
        /// </summary>
        public ObservableCollection<MovieGenre> MovieGenres
        {
            get { return _movieGenres; }
            set { Set(() => MovieGenres, ref _movieGenres, value); }
        }

        /// <summary>
        /// Load genres asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task LoadGenresAsync()
        {
            MovieGenres =
                new ObservableCollection<MovieGenre>(
                    await _movieService.GetGenresAsync(_cancellationLoadingGenres.Token));
            if (_cancellationLoadingGenres.IsCancellationRequested)
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
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            StopLoadingGenres();
            base.Cleanup();
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
        /// Cancel the loading of genres
        /// </summary>
        private void StopLoadingGenres()
        {
            Logger.Debug(
                "Stop loading genres.");

            _cancellationLoadingGenres.Cancel(true);
            _cancellationLoadingGenres = new CancellationTokenSource();
        }
    }
}