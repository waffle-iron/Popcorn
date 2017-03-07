using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Movie;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Pages.Player;
using YoutubeExtractor;

namespace Popcorn.ViewModels.Pages.Home.Movie.Trailer
{
    /// <summary>
    /// Manage trailer
    /// </summary>
    public sealed class TrailerViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

        /// <summary>
        /// The service used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        /// <summary>
        /// Manage the trailer player
        /// </summary>
        private MediaPlayerViewModel _trailerPlayer;

        /// <summary>
        /// Initializes a new instance of the TrailerViewModel class.
        /// </summary>
        /// <param name="movieService">Movie service</param>
        public TrailerViewModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Manage the trailer player
        /// </summary>
        public MediaPlayerViewModel TrailerPlayer
        {
            get { return _trailerPlayer; }
            set { Set(() => TrailerPlayer, ref _trailerPlayer, value); }
        }

        /// <summary>
        /// Load movie's trailer asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Cancellation token</param>
        public async Task LoadTrailerAsync(MovieJson movie, CancellationToken ct)
        {
            try
            {
                var trailer = await _movieService.GetMovieTrailerAsync(movie, ct);

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
                    TrailerPlayer = new MediaPlayerViewModel(video.DownloadUrl, movie.Title,
                        () =>
                        {
                            Messenger.Default.Send(new StopPlayingTrailerMessage());
                        },
                        () =>
                        {
                            Messenger.Default.Send(new StopPlayingTrailerMessage());
                        });
                }
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetMovieTrailerAsync cancelled.");
                Messenger.Default.Send(new StopPlayingTrailerMessage());
            }
            catch (Exception exception) when (exception is SocketException || exception is WebException)
            {
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                Messenger.Default.Send(new StopPlayingTrailerMessage());
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            catch (Exception exception)
                when (exception is VideoNotAvailableException || exception is YoutubeParseException)
            {
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
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                Messenger.Default.Send(new StopPlayingTrailerMessage());
            }
        }

        /// <summary>
        /// Unload the trailer
        /// </summary>
        public void UnLoadTrailer()
        {
            TrailerPlayer = null;
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        public override void Cleanup()
        {
            TrailerPlayer = null;
            base.Cleanup();
        }

        /// <summary>
        /// Get VideoInfo of a youtube video
        /// </summary>
        /// <param name="youtubeLink">The youtube link of a movie</param>
        /// <param name="qualitySetting">The youtube quality settings</param>
        /// <returns>The trailer's video info</returns>
        private async Task<VideoInfo> GetVideoInfoForStreamingAsync(string youtubeLink,
            Constants.YoutubeStreamingQuality qualitySetting)
        {
            IEnumerable<VideoInfo> videoInfos = new List<VideoInfo>();

            // Get video infos of the requested video
            await Task.Run(() => videoInfos = DownloadUrlResolver.GetDownloadUrls(youtubeLink, false));

            // We only want video matching criterias : only mp4 and no adaptive
            var filtered = videoInfos
                .Where(info => info.VideoType == VideoType.Mp4 && !info.Is3D && info.AdaptiveType == AdaptiveType.None);

            return GetVideoByStreamingQuality(filtered, qualitySetting);
        }

        /// <summary>
        /// Get youtube video depending of choosen quality settings
        /// </summary>
        /// <param name="videosToProcess">List of VideoInfo</param>
        /// <param name="quality">The youtube quality settings</param>
        /// <returns>The trailer's video info</returns>
        private VideoInfo GetVideoByStreamingQuality(IEnumerable<VideoInfo> videosToProcess,
            Constants.YoutubeStreamingQuality quality)
        {
            while (true)
            {
                var videos = videosToProcess.ToList(); // Prevent multiple enumeration

                if (quality == Constants.YoutubeStreamingQuality.High)
                    // Choose high quality Youtube video
                    return videos.OrderByDescending(x => x.Resolution).FirstOrDefault();

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
    }
}