using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using lt;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Movie;
using Popcorn.Services.Language;
using Popcorn.Services.Subtitles;
using Popcorn.ViewModels.Pages.Home.Movie.Settings;
using Popcorn.ViewModels.Pages.Home.Movie.Subtitles;
using Popcorn.ViewModels.Windows.Settings;

namespace Popcorn.ViewModels.Pages.Home.Movie.Download
{
    /// <summary>
    /// Manage the download of a movie
    /// </summary>
    public sealed class DownloadMovieViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used to interact with subtitles
        /// </summary>
        private readonly ISubtitlesService _subtitlesService;

        /// <summary>
        /// Manage th application settings
        /// </summary>
        private readonly ApplicationSettingsViewModel _applicationSettingsViewModel;

        /// <summary>
        /// Token to cancel the download
        /// </summary>
        private CancellationTokenSource _cancellationDownloadingMovie;

        /// <summary>
        /// Specify if a movie is downloading
        /// </summary>
        private bool _isDownloadingMovie;

        /// <summary>
        /// Specify if subtitles are downloading
        /// </summary>
        private bool _isDownloadingSubtitles;

        /// <summary>
        /// Specify if a movie is buffered
        /// </summary>
        private bool _isMovieBuffered;

        /// <summary>
        /// The movie to download
        /// </summary>
        private MovieJson _movie;

        /// <summary>
        /// The movie download progress
        /// </summary>
        private double _movieDownloadProgress;

        /// <summary>
        /// The movie download rate
        /// </summary>
        private double _movieDownloadRate;

        /// <summary>
        /// Manage the movie's settings
        /// </summary>
        private MovieSettingsViewModel _movieSettings;

        /// <summary>
        /// Initializes a new instance of the DownloadMovieViewModel class.
        /// </summary>
        /// <param name="subtitlesService">Instance of SubtitlesService</param>
        /// <param name="languageService">Language service</param>
        public DownloadMovieViewModel(ISubtitlesService subtitlesService, ILanguageService languageService)
        {
            _subtitlesService = subtitlesService;
            _applicationSettingsViewModel = new ApplicationSettingsViewModel(languageService);
            _cancellationDownloadingMovie = new CancellationTokenSource();
            MovieSettings = new MovieSettingsViewModel(new SubtitlesViewModel(_subtitlesService));
            RegisterMessages();
            RegisterCommands();
        }

        /// <summary>
        /// Manage the movie's settings
        /// </summary>
        public MovieSettingsViewModel MovieSettings
        {
            get { return _movieSettings; }
            set { Set(() => MovieSettings, ref _movieSettings, value); }
        }

        /// <summary>
        /// Specify if a movie is downloading
        /// </summary>
        public bool IsDownloadingMovie
        {
            get { return _isDownloadingMovie; }
            set { Set(() => IsDownloadingMovie, ref _isDownloadingMovie, value); }
        }

        /// <summary>
        /// Specify if subtitles are downloading
        /// </summary>
        public bool IsDownloadingSubtitles
        {
            get { return _isDownloadingSubtitles; }
            set { Set(() => IsDownloadingSubtitles, ref _isDownloadingSubtitles, value); }
        }

        /// <summary>
        /// Specify the movie download progress
        /// </summary>
        public double MovieDownloadProgress
        {
            get { return _movieDownloadProgress; }
            set { Set(() => MovieDownloadProgress, ref _movieDownloadProgress, value); }
        }

        /// <summary>
        /// Specify the movie download rate
        /// </summary>
        public double MovieDownloadRate
        {
            get { return _movieDownloadRate; }
            set { Set(() => MovieDownloadRate, ref _movieDownloadRate, value); }
        }

        /// <summary>
        /// The movie to download
        /// </summary>
        public MovieJson Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// The command used to stop the download of a movie
        /// </summary>
        public RelayCommand StopDownloadingMovieCommand { get; private set; }

        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        public void LoadMovie(MovieJson movie)
        {
            MovieDownloadProgress = 0d;
            MovieDownloadRate = 0d;
            Movie = movie;
            MovieSettings.LoadMovie(Movie);
        }

        /// <summary>
        /// Stop downloading a movie
        /// </summary>
        public void StopDownloadingMovie()
        {
            Logger.Info(
                "Stop downloading a movie");

            IsDownloadingMovie = false;
            _isMovieBuffered = false;
            _cancellationDownloadingMovie.Cancel(true);
            _cancellationDownloadingMovie = new CancellationTokenSource();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            StopDownloadingMovie();
            MovieSettings?.Cleanup();
            base.Cleanup();
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages() => Messenger.Default.Register<DownloadMovieMessage>(
            this,
            message =>
            {
                var reportDownloadProgress = new Progress<double>(ReportMovieDownloadProgress);
                var reportDownloadRate = new Progress<double>(ReportMovieDownloadRate);

                IsDownloadingSubtitles = true;
                Task.Run(() =>
                {
                    try
                    {
                        var path = Path.Combine(Constants.Subtitles + message.Movie.ImdbCode);
                        Directory.CreateDirectory(path);
                        var subtitlePath =
                            _subtitlesService.DownloadSubtitleToPath(path,
                                message.Movie.SelectedSubtitle.Sub);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            message.Movie.SelectedSubtitle.FilePath = subtitlePath;
                            IsDownloadingSubtitles = false;
                        });
                    }
                    catch (Exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            IsDownloadingSubtitles = false;
                        });
                    }
                    finally
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                        {
                            await
                            DownloadMovieAsync(message.Movie,
                                reportDownloadProgress, reportDownloadRate, _cancellationDownloadingMovie);
                        });
                    }
                });
            });

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands() => StopDownloadingMovieCommand = new RelayCommand(StopDownloadingMovie);

        /// <summary>
        /// Report the download progress
        /// </summary>
        /// <param name="value">Download rate</param>
        private void ReportMovieDownloadRate(double value) => MovieDownloadRate = value;

        /// <summary>
        /// Report the download progress
        /// </summary>
        /// <param name="value">The download progress to report</param>
        private void ReportMovieDownloadProgress(double value)
        {
            MovieDownloadProgress = value;
            if (value < Constants.MinimumBufferingBeforeMoviePlaying)
                return;

            if (!_isMovieBuffered)
                _isMovieBuffered = true;
        }

        /// <summary>
        /// Download a movie asynchronously
        /// </summary>
        /// <param name="movie">The movie to download</param>
        /// <param name="downloadProgress">Report download progress</param>
        /// <param name="downloadRate">Report download rate</param>
        /// <param name="ct">Cancellation token</param>
        private async Task DownloadMovieAsync(MovieJson movie, IProgress<double> downloadProgress,
            IProgress<double> downloadRate,
            CancellationTokenSource ct)
        {
            await Task.Run(async () =>
            {
                using (var session = new session())
                {
                    Logger.Info(
                        $"Start downloading movie : {movie.Title}");

                    IsDownloadingMovie = true;

                    downloadProgress?.Report(0d);
                    downloadRate?.Report(0d);

                    session.listen_on(6881, 6889);
                    var torrentUrl = movie.WatchInFullHdQuality
                        ? movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "1080p")?.Url
                        : movie.Torrents?.FirstOrDefault(torrent => torrent.Quality == "720p")?.Url;

                    var result =
                        await
                            DownloadFileHelper.DownloadFileTaskAsync(torrentUrl,
                                Constants.TorrentDownloads + movie.ImdbCode + ".torrent", ct: ct);
                    var torrentPath = string.Empty;
                    if (result.Item3 == null && !string.IsNullOrEmpty(result.Item2))
                        torrentPath = result.Item2;

                    var addParams = new add_torrent_params
                    {
                        save_path = Constants.MovieDownloads,
                        ti = new torrent_info(torrentPath)
                    };

                    var handle = session.add_torrent(addParams);
                    handle.set_upload_limit(_applicationSettingsViewModel.DownloadLimit * 1024);
                    handle.set_download_limit(_applicationSettingsViewModel.UploadLimit * 1024);

                    // We have to download sequentially, so that we're able to play the movie without waiting
                    handle.set_sequential_download(true);
                    var alreadyBuffered = false;
                    while (IsDownloadingMovie)
                    {
                        var status = handle.status();
                        var progress = status.progress * 100d;

                        downloadProgress?.Report(progress);
                        downloadRate?.Report(Math.Round(status.download_rate / 1024d, 0));

                        handle.flush_cache();
                        if (handle.need_save_resume_data())
                            handle.save_resume_data(1);

                        if (progress >= Constants.MinimumBufferingBeforeMoviePlaying && !alreadyBuffered)
                        {
                            // Get movie file
                            foreach (
                                var filePath in
                                Directory.GetFiles(status.save_path + handle.torrent_file().name(),
                                    "*" + Constants.VideoFileExtension)
                            )
                            {
                                alreadyBuffered = true;
                                movie.FilePath = new Uri(filePath);
                                Messenger.Default.Send(new PlayMovieMessage(movie));
                            }
                        }

                        try
                        {
                            await Task.Delay(1000, ct.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            return;
                        }
                    }
                }
            }, ct.Token);
        }
    }
}