using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Models.Movie.Full;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Players.Trailer;
using YoutubeExtractor;

namespace Popcorn.ViewModels.Trailer
{
    /// <summary>
    /// Manage trailer
    /// </summary>
    public sealed class TrailerViewModel : ViewModelBase
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        #region Property -> MovieService

        /// <summary>
        /// The service used to interact with movies
        /// </summary>
        private MovieService MovieService { get; }

        #endregion

        #region Property -> Movie

        /// <summary>
        /// Represent the subtitle's movie
        /// </summary>
        private MovieFull Movie { get; }

        #endregion

        #region Property -> StreamingQualityMap

        /// <summary>
        /// Map for defining youtube video quality
        /// </summary>
        private static readonly IReadOnlyDictionary<Constants.YoutubeStreamingQuality, IEnumerable<int>>
            StreamingQualityMap =
                new Dictionary<Constants.YoutubeStreamingQuality, IEnumerable<int>>
                {
                    {Constants.YoutubeStreamingQuality.High, new HashSet<int> {1080, 720}},
                    {Constants.YoutubeStreamingQuality.Medium, new HashSet<int> {480}},
                    {Constants.YoutubeStreamingQuality.Low, new HashSet<int> {360, 240}}
                };

        #endregion

        #region Property -> TrailerPlayer

        private TrailerPlayerViewModel _trailerPlayer;

        /// <summary>
        /// The trailer player
        /// </summary>
        public TrailerPlayerViewModel TrailerPlayer
        {
            get { return _trailerPlayer; }
            set { Set(() => TrailerPlayer, ref _trailerPlayer, value); }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TrailerViewModel class.
        /// </summary>
        /// <param name="movie">Movie's trailer</param>
        private TrailerViewModel(MovieFull movie)
        {
            if (SimpleIoc.Default.IsRegistered<MovieService>())
                MovieService = SimpleIoc.Default.GetInstance<MovieService>();

            Movie = movie;
        }

        #endregion

        #region Method -> InitializeAsync

        /// <summary>
        /// Load asynchronously the movie's trailer for the current instance of TrailerViewModel
        /// </summary>
        /// <returns>Instance of TrailerViewModel</returns>
        private async Task<TrailerViewModel> InitializeAsync(CancellationToken ct)
        {
            await LoadTrailerAsync(Movie, ct);
            return this;
        }

        #endregion

        #region Method -> CreateAsync

        /// <summary>
        /// Initialize asynchronously an instance of the TrailerViewModel class
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Instance of TrailerViewModel</returns>
        public static Task<TrailerViewModel> CreateAsync(MovieFull movie, CancellationToken ct)
        {
            var ret = new TrailerViewModel(movie);
            return ret.InitializeAsync(ct);
        }

        #endregion

        #region Method -> LoadTrailerAsync

        /// <summary>
        /// Get trailer of a movie
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Cancellation token</param>
        private async Task LoadTrailerAsync(MovieFull movie, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                var trailer = await MovieService.GetMovieTrailerAsync(movie, ct);

                var video =
                    await
                        GetVideoInfoForStreamingAsync(
                            Constants.YoutubePath + trailer.Results.FirstOrDefault()?.Key,
                            Constants.YoutubeStreamingQuality.High);

                if (video != null && video.RequiresDecryption)
                {
                    Logger.Info(
                        $"Decrypting Youtube trailer url: {video.Title}");
                    await Task.Run(() => DownloadUrlResolver.DecryptDownloadUrl(video), ct);
                }

                if (video == null)
                {
                    Logger.Error(
                        $"Failed loading movie's trailer: {movie.Title}");
                    Messenger.Default.Send(
                        new ManageExceptionMessage(
                            new Exception(
                                LocalizationProviderHelper.GetLocalizedValue<string>("TrailerNotAvailable"))));
                    Messenger.Default.Send(new StopPlayingTrailerMessage());
                    return;
                }

                if (!ct.IsCancellationRequested)
                {
                    Logger.Debug(
                        $"Movie's trailer loaded: {movie.Title}");
                    TrailerPlayer =
                        new TrailerPlayerViewModel(new Models.Trailer.Trailer(new Uri(video.DownloadUrl)));
                }
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                watch.Stop();
                Logger.Debug(
                    "GetMovieTrailerAsync cancelled.");
                Messenger.Default.Send(new StopPlayingTrailerMessage());

            }
            catch (Exception exception) when (exception is SocketException || exception is WebException)
            {
                watch.Stop();
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                Messenger.Default.Send(new StopPlayingTrailerMessage());
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            catch (Exception exception)
                when (exception is VideoNotAvailableException || exception is YoutubeParseException)
            {
                watch.Stop();
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                Messenger.Default.Send(
                    new ManageExceptionMessage(
                        new Exception(
                            LocalizationProviderHelper.GetLocalizedValue<string>(
                                "TrailerNotAvailable"))));
                Messenger.Default.Send(new StopPlayingTrailerMessage());
            }
            catch (Exception exception)
            {
                watch.Stop();
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                Messenger.Default.Send(new StopPlayingTrailerMessage());
            }
        }

        #endregion

        #region Method -> GetVideoInfoForStreamingAsync

        /// <summary>
        /// Get VideoInfo of a youtube video
        /// </summary>
        /// <param name="youtubeLink">The youtube link of a movie</param>
        /// <param name="qualitySetting">The youtube quality settings</param>
        private async Task<VideoInfo> GetVideoInfoForStreamingAsync(string youtubeLink,
            Constants.YoutubeStreamingQuality qualitySetting)
        {
            IEnumerable<VideoInfo> videoInfos = new List<VideoInfo>();

            // Get video infos of the requested video
            await Task.Run(() =>
            {
                videoInfos = DownloadUrlResolver.GetDownloadUrls(youtubeLink, false);
            });

            // We only want video matching criterias : only mp4 and no adaptive
            var filtered = videoInfos
                .Where(info => info.VideoType == VideoType.Mp4 && !info.Is3D && info.AdaptiveType == AdaptiveType.None);

            return GetVideoByStreamingQuality(filtered, qualitySetting);
        }

        #endregion

        #region Method -> GetVideoByStreamingQuality

        /// <summary>
        /// Get youtube video depending of choosen quality settings
        /// </summary>
        /// <param name="videosToProcess">List of VideoInfo</param>
        /// <param name="quality">The youtube quality settings</param>
        private VideoInfo GetVideoByStreamingQuality(IEnumerable<VideoInfo> videosToProcess,
            Constants.YoutubeStreamingQuality quality)
        {
            while (true)
            {
                var videos = videosToProcess.ToList(); // Prevent multiple enumeration

                if (quality == Constants.YoutubeStreamingQuality.High)
                {
                    // Choose high quality Youtube video
                    return videos.OrderByDescending(x => x.Resolution).FirstOrDefault();
                }

                // Pick the video with the requested quality settings
                var preferredResolutions = StreamingQualityMap[quality];

                var preferredVideos =
                    videos.Where(info => preferredResolutions.Contains(info.Resolution))
                        .OrderByDescending(info => info.Resolution);

                var video = preferredVideos.FirstOrDefault();

                if (video != null) return video;
                videosToProcess = videos;
                quality = (Constants.YoutubeStreamingQuality) (((int) quality) - 1);
            }
        }

        #endregion

        public override void Cleanup()
        {
            Logger.Debug(
                "Cleaning up TrailerViewModel");
            TrailerPlayer?.Cleanup();

            base.Cleanup();
        }
    }
}