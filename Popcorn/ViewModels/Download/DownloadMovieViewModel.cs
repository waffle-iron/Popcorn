using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.ViewModels.MovieSettings;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Models.Movie.Full;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Settings;
using lt;

namespace Popcorn.ViewModels.Download
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
        /// The service used to interact with movies
        /// </summary>
        private MovieService MovieService { get; }

        private MovieSettingsViewModel _movieSettings;

        /// <summary>
        /// The view model used to manage movie's settings
        /// </summary>
        public MovieSettingsViewModel MovieSettings
        {
            get { return _movieSettings; }
            set { Set(() => MovieSettings, ref _movieSettings, value); }
        }

        private bool _isDownloadingMovie;

        /// <summary>
        /// Specify if a movie is downloading
        /// </summary>
        public bool IsDownloadingMovie
        {
            get { return _isDownloadingMovie; }
            set { Set(() => IsDownloadingMovie, ref _isDownloadingMovie, value); }
        }

        private bool _isDownloadingSubtitles;

        /// <summary>
        /// Specify if subtitles are downloading
        /// </summary>
        public bool IsDownloadingSubtitles
        {
            get { return _isDownloadingSubtitles; }
            set { Set(() => IsDownloadingSubtitles, ref _isDownloadingSubtitles, value); }
        }

        private bool IsMovieBuffered { get; set; }

        private double _movieDownloadProgress;

        /// <summary>
        /// Specify the movie download progress
        /// </summary>
        public double MovieDownloadProgress
        {
            get { return _movieDownloadProgress; }
            set { Set(() => MovieDownloadProgress, ref _movieDownloadProgress, value); }
        }

        private double _movieDownloadRate;

        /// <summary>
        /// Specify the movie download rate
        /// </summary>
        public double MovieDownloadRate
        {
            get { return _movieDownloadRate; }
            set { Set(() => MovieDownloadRate, ref _movieDownloadRate, value); }
        }

        private long _subtitlesDownloadProgress;

        /// <summary>
        /// Specify the subtitles' progress download
        /// </summary>
        public long SubtitlesDownloadProgress
        {
            get { return _subtitlesDownloadProgress; }
            set { Set(() => SubtitlesDownloadProgress, ref _subtitlesDownloadProgress, value); }
        }

        private MovieFull _movie;

        /// <summary>
        /// The movie to download
        /// </summary>
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        /// <summary>
        /// Token to cancel the download
        /// </summary>
        private CancellationTokenSource CancellationDownloadingMovie { get; set; }

        /// <summary>
        /// The command used to stop the download of a movie
        /// </summary>
        public RelayCommand StopDownloadingMovieCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DownloadMovieViewModel class.
        /// </summary>
        /// <param name="movie">The movie to download</param>
        public DownloadMovieViewModel(MovieFull movie)
        {
            RegisterMessages();
            RegisterCommands();
            CancellationDownloadingMovie = new CancellationTokenSource();
            if (SimpleIoc.Default.IsRegistered<MovieService>())
                MovieService = SimpleIoc.Default.GetInstance<MovieService>();

            Movie = movie;
            MovieSettings = new MovieSettingsViewModel(movie);
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<DownloadMovieMessage>(
                this,
                async message =>
                {
                    var reportDownloadProgress = new Progress<double>(ReportMovieDownloadProgress);
                    var reportDownloadRate = new Progress<double>(ReportMovieDownloadRate);
                    var reportDownloadSubtitles = new Progress<long>(ReportSubtitlesDownloadProgress);

                    IsDownloadingSubtitles = true;
                    await
                        MovieService.DownloadSubtitleAsync(message.Movie, reportDownloadSubtitles,
                            CancellationDownloadingMovie);
                    IsDownloadingSubtitles = false;
                    await
                        DownloadMovieAsync(message.Movie,
                            reportDownloadProgress, reportDownloadRate, CancellationDownloadingMovie);
                });
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            StopDownloadingMovieCommand = new RelayCommand(StopDownloadingMovie);
        }

        /// <summary>
        /// Report the download progress
        /// </summary>
        /// <param name="value"></param>
        private void ReportMovieDownloadRate(double value)
        {
            MovieDownloadRate = value;
        }

        /// <summary>
        /// Report the download progress of the subtitles
        /// </summary>
        /// <param name="value"></param>
        private void ReportSubtitlesDownloadProgress(long value)
        {
            SubtitlesDownloadProgress = value;
        }

        /// <summary>
        /// Report the download progress
        /// </summary>
        /// <param name="value">The value to report</param>
        private void ReportMovieDownloadProgress(double value)
        {
            MovieDownloadProgress = value;
            if (value < Constants.MinimumBufferingBeforeMoviePlaying)
                return;

            if (!IsMovieBuffered)
            {
                IsMovieBuffered = true;
            }
        }

        /// <summary>
        /// Download a movie
        /// </summary>
        /// <param name="movie">The movie to download</param>
        /// <param name="downloadProgress">Report download progress</param>
        /// <param name="downloadRate">Report download rate</param>
        /// <param name="ct">Cancellation token</param>
        private async Task DownloadMovieAsync(MovieFull movie, IProgress<double> downloadProgress,
            IProgress<double> downloadRate,
            CancellationTokenSource ct)
        {
            await Task.Run(async () =>
            {
                using (var session = new session())
                {
                    Logger.Debug(
                        $"Start downloading movie : {movie.Title}");

                    IsDownloadingMovie = true;

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
                    handle.set_upload_limit(SimpleIoc.Default.IsRegistered<SettingsViewModel>()
                        ? SimpleIoc.Default.GetInstance<SettingsViewModel>().DownloadLimit*1024
                        : 0);
                    handle.set_download_limit(SimpleIoc.Default.IsRegistered<SettingsViewModel>()
                        ? SimpleIoc.Default.GetInstance<SettingsViewModel>().UploadLimit*1024
                        : 0);

                    // We have to download sequentially, so that we're able to play the movie without waiting
                    handle.set_sequential_download(true);
                    var alreadyBuffered = false;
                    while (IsDownloadingMovie)
                    {
                        var status = handle.status();
                        var progress = status.progress*100.0;

                        downloadProgress?.Report(progress);
                        downloadRate?.Report(Math.Round(status.download_rate/1024.0, 0));

                        handle.flush_cache();
                        if (handle.need_save_resume_data())
                        {
                            handle.save_resume_data(1);
                        }

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
                            Logger.Debug(
                                $"Stopped downloading movie : {movie.Title}");
                            return;
                        }
                    }
                }
            }, ct.Token);
        }

        /// <summary>
        /// Stop downloading a movie
        /// </summary>
        private void StopDownloadingMovie()
        {
            Logger.Debug(
                "Stop downloading movie");

            IsDownloadingMovie = false;
            IsMovieBuffered = false;
            CancellationDownloadingMovie.Cancel(true);
            CancellationDownloadingMovie = new CancellationTokenSource();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up DownloadMovieViewModel");

            StopDownloadingMovie();
            MovieSettings?.Cleanup();
            base.Cleanup();
        }
    }
}