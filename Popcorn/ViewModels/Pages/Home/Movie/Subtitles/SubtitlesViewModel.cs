using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using NLog;
using Popcorn.Models.Movie;
using Popcorn.Models.Subtitles;
using Popcorn.Services.Subtitles;

namespace Popcorn.ViewModels.Pages.Home.Movie.Subtitles
{
    public sealed class SubtitlesViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The service used to interact with subtitles
        /// </summary>
        private readonly ISubtitlesService _subtitlesService;

        /// <summary>
        /// Token to cancel downloading subtitles
        /// </summary>
        private CancellationTokenSource _cancellationDownloadingSubtitlesToken;

        /// <summary>
        /// Specify if movie's subtitles are enabled
        /// </summary>
        private bool _enabledSubtitles;

        /// <summary>
        /// The movie to manage
        /// </summary>
        private MovieJson _movie;

        /// <summary>
        /// True if subtitles are loading
        /// </summary>
        private bool _loadingSubtitles;

        /// <summary>
        /// Initializes a new instance of the SubtitlesViewModel class.
        /// </summary>
        /// <param name="subtitlesService">The subtitles service</param>
        public SubtitlesViewModel(ISubtitlesService subtitlesService)
        {
            _subtitlesService = subtitlesService;
            _cancellationDownloadingSubtitlesToken = new CancellationTokenSource();
        }

        /// <summary>
        /// The movie to manage
        /// </summary>
        public MovieJson Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// True if subtitles are loading
        /// </summary>
        public bool LoadingSubtitles
        {
            get { return _loadingSubtitles; }
            set { Set(() => LoadingSubtitles, ref _loadingSubtitles, value); }
        }

        /// <summary>
        /// Specify if movie's subtitles are enabled
        /// </summary>
        public bool EnabledSubtitles
        {
            get { return _enabledSubtitles; }
            set { Set(() => EnabledSubtitles, ref _enabledSubtitles, value); }
        }

        /// <summary>
        /// Load the movie's subtitles asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        public void LoadSubtitles(MovieJson movie)
        {
            Logger.Debug(
                $"Load subtitles for movie: {movie.Title}");
            Movie = movie;
            EnabledSubtitles = true;
            LoadingSubtitles = true;
            Task.Run(() =>
            {
                try
                {
                    var languages = _subtitlesService.GetSubLanguages();

                    var imdbId = 0;
                    if (int.TryParse(new string(movie.ImdbCode
                        .SkipWhile(x => !char.IsDigit(x))
                        .TakeWhile(char.IsDigit)
                        .ToArray()), out imdbId))
                    {
                        var subtitles = _subtitlesService.SearchSubtitlesFromImdb(
                            languages.Select(lang => lang.SubLanguageID).Aggregate((a, b) => a + "," + b),
                            imdbId.ToString());
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            movie.AvailableSubtitles =
                                new ObservableCollection<Subtitle>(subtitles.Select(sub => new Subtitle
                                {
                                    Sub = sub
                                }).GroupBy(x => x.Sub.LanguageName,
                                    (k, g) =>
                                        g.Aggregate(
                                            (a, x) =>
                                                (Convert.ToDouble(x.Sub.Rating, CultureInfo.InvariantCulture) >=
                                                 Convert.ToDouble(a.Sub.Rating, CultureInfo.InvariantCulture))
                                                    ? x
                                                    : a)));
                            LoadingSubtitles = false;
                        });
                    }
                }
                catch (Exception)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        LoadingSubtitles = false;
                    });
                }
            });
        }

        /// <summary>
        /// Stop downloading subtitles and clear movie
        /// </summary>
        public void ClearSubtitles()
        {
            EnabledSubtitles = false;
            StopDownloadingSubtitles();
            if (Movie != null)
                Movie.SelectedSubtitle = null;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            ClearSubtitles();
            base.Cleanup();
        }

        /// <summary>
        /// Stop downloading subtitles
        /// </summary>
        private void StopDownloadingSubtitles()
        {
            Logger.Debug(
                "Stop downloading subtitles");
            _cancellationDownloadingSubtitlesToken.Cancel(true);
            _cancellationDownloadingSubtitlesToken = new CancellationTokenSource();
        }
    }
}