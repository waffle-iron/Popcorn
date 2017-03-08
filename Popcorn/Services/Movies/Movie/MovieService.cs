using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Popcorn.Helpers;
using Popcorn.Models.Genre;
using Popcorn.Models.Localization;
using Popcorn.Models.Movie;
using Popcorn.Models.Subtitle;
using RestSharp;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace Popcorn.Services.Movies.Movie
{
    /// <summary>
    /// Services used to interact with movies
    /// </summary>
    public class MovieService : IMovieService
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initialize a new instance of MovieService class
        /// </summary>
        public MovieService()
        {
            TmdbClient = new TMDbClient(Constants.TmDbClientId)
            {
                MaxRetryCount = 10
            };
        }

        /// <summary>
        /// TMDb client
        /// </summary>
        private TMDbClient TmdbClient { get; }

        /// <summary>
        /// Change the culture of TMDb
        /// </summary>
        /// <param name="language">Language to set</param>
        public void ChangeTmdbLanguage(ILanguage language) => TmdbClient.DefaultLanguage = language.Culture;

        /// <summary>
        /// Get all movie's genres
        /// </summary>
        /// <param name="ct">Used to cancel loading genres</param>
        /// <returns>Genres</returns>
        public async Task<List<GenreJson>> GetGenresAsync(CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var genres = new List<GenreJson>();

            try
            {
                await Task.Run(async () =>
                {
                    var englishGenre = await TmdbClient.GetMovieGenresAsync(new EnglishLanguage().Culture);
                    genres.AddRange((await TmdbClient.GetMovieGenresAsync()).Select(genre => new GenreJson
                    {
                        EnglishName = englishGenre.FirstOrDefault(p => p.Id == genre.Id)?.Name,
                        TmdbGenre = genre
                    }));
                }, ct);
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetGenresAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"GetGenresAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"GetGenresAsync in {elapsedMs} milliseconds.");
            }

            return genres;
        }

        /// <summary>
        /// Get popular movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Popular movies and the number of movies found</returns>
        public async Task<Tuple<IEnumerable<MovieJson>, int>> GetPopularMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            GenreJson genre = null)
        {
            var watch = Stopwatch.StartNew();

            var wrapper = new MovieResponse();

            if (limit < 1 || limit > 50)
                limit = 20;

            if (page < 1)
                page = 1;

            var restClient = new RestClient(Constants.PopcornApi);
            var request = new RestRequest("/{segment}", Method.GET);
            request.AddUrlSegment("segment", "movies");
            request.AddParameter("limit", limit);
            request.AddParameter("page", page);
            if (genre != null) request.AddParameter("genre", genre.EnglishName);
            request.AddParameter("minimum_rating", ratingFilter);
            request.AddParameter("sort_by", "like_count");

            try
            {
                var response = await restClient.ExecuteGetTaskAsync<MovieResponse>(request, ct);
                if (response.ErrorException != null)
                    throw response.ErrorException;

                wrapper = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetPopularMoviesAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"GetPopularMoviesAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"GetPopularMoviesAsync ({page}, {limit}) in {elapsedMs} milliseconds.");
            }

            var movies = wrapper.Movies ?? new List<MovieJson>();
            var nbMovies = wrapper.TotalMovies;

            return new Tuple<IEnumerable<MovieJson>, int>(movies, nbMovies);
        }

        /// <summary>
        /// Get greatest movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Top rated movies and the number of movies found</returns>
        public async Task<Tuple<IEnumerable<MovieJson>, int>> GetGreatestMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            GenreJson genre = null)
        {
            var watch = Stopwatch.StartNew();

            var wrapper = new MovieResponse();

            if (limit < 1 || limit > 50)
                limit = 20;

            if (page < 1)
                page = 1;

            var restClient = new RestClient(Constants.PopcornApi);
            var request = new RestRequest("/{segment}", Method.GET);
            request.AddUrlSegment("segment", "movies");
            request.AddParameter("limit", limit);
            request.AddParameter("page", page);
            if (genre != null) request.AddParameter("genre", genre.EnglishName);
            request.AddParameter("minimum_rating", ratingFilter);
            request.AddParameter("sort_by", "download_count");

            try
            {
                var response = await restClient.ExecuteGetTaskAsync<MovieResponse>(request, ct);
                if (response.ErrorException != null)
                    throw response.ErrorException;

                wrapper = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetGreatestMoviesAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"GetGreatestMoviesAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"GetGreatestMoviesAsync ({page}, {limit}) in {elapsedMs} milliseconds.");
            }

            var movies = wrapper.Movies ?? new List<MovieJson>();
            var nbMovies = wrapper.TotalMovies;

            return new Tuple<IEnumerable<MovieJson>, int>(movies, nbMovies);
        }

        /// <summary>
        /// Get recent movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Recent movies and the number of movies found</returns>
        public async Task<Tuple<IEnumerable<MovieJson>, int>> GetRecentMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            GenreJson genre = null)
        {
            var watch = Stopwatch.StartNew();

            var wrapper = new MovieResponse();

            if (limit < 1 || limit > 50)
                limit = 20;

            if (page < 1)
                page = 1;

            var restClient = new RestClient(Constants.PopcornApi);
            var request = new RestRequest("/{segment}", Method.GET);
            request.AddUrlSegment("segment", "movies");
            request.AddParameter("limit", limit);
            request.AddParameter("page", page);
            if (genre != null) request.AddParameter("genre", genre.EnglishName);
            request.AddParameter("minimum_rating", ratingFilter);
            request.AddParameter("sort_by", "seeds");

            try
            {
                var response = await restClient.ExecuteGetTaskAsync<MovieResponse>(request, ct);
                if (response.ErrorException != null)
                    throw response.ErrorException;

                wrapper = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetRecentMoviesAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"GetRecentMoviesAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"GetRecentMoviesAsync ({page}, {limit}) in {elapsedMs} milliseconds.");
            }

            var movies = wrapper.Movies ?? new List<MovieJson>();
            var nbMovies = wrapper.TotalMovies;

            return new Tuple<IEnumerable<MovieJson>, int>(movies, nbMovies);
        }

        /// <summary>
        /// Search movies by criteria
        /// </summary>
        /// <param name="criteria">Criteria used for search</param>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Searched movies and the number of movies found</returns>
        public async Task<Tuple<IEnumerable<MovieJson>, int>> SearchMoviesAsync(string criteria,
            int page,
            int limit,
            GenreJson genre,
            double ratingFilter,
            CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var wrapper = new MovieResponse();

            if (limit < 1 || limit > 50)
                limit = 20;

            if (page < 1)
                page = 1;

            var restClient = new RestClient(Constants.PopcornApi);
            var request = new RestRequest("/{segment}", Method.GET);
            request.AddUrlSegment("segment", "movies");
            request.AddParameter("limit", limit);
            request.AddParameter("page", page);
            if (genre != null) request.AddParameter("genre", genre.EnglishName);
            request.AddParameter("minimum_rating", ratingFilter);
            request.AddParameter("query_term", criteria);

            try
            {
                var response = await restClient.ExecuteGetTaskAsync<MovieResponse>(request, ct);
                if (response.ErrorException != null)
                    throw response.ErrorException;

                wrapper = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "SearchMoviesAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"SearchMoviesAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"SearchMoviesAsync ({criteria}, {page}, {limit}) in {elapsedMs} milliseconds.");
            }

            var movies = wrapper.Movies ?? new List<MovieJson>();
            var nbMovies = wrapper.TotalMovies;

            return new Tuple<IEnumerable<MovieJson>, int>(movies, nbMovies);
        }

        /// <summary>
        /// Translate movie informations (title, description, ...)
        /// </summary>
        /// <param name="movieToTranslate">Movie to translate</param>
        /// <param name="ct">Used to cancel translation</param>
        /// <returns>Task</returns>
        public async Task TranslateMovieAsync(MovieJson movieToTranslate, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                await Task.Run(async () =>
                {
                    var movie = await TmdbClient.GetMovieAsync(movieToTranslate.ImdbCode,
                        MovieMethods.Credits);
                    movieToTranslate.Title = movie?.Title;
                    movieToTranslate.Genres = movie?.Genres?.Select(a => a.Name).ToList();
                    movieToTranslate.DescriptionFull = movie?.Overview;
                }, ct);
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "TranslateMovieAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"TranslateMovieAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"TranslateMovieAsync ({movieToTranslate.ImdbCode}) in {elapsedMs} milliseconds.");
            }
        }

        /// <summary>
        /// Get the link to the youtube trailer of a movie
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Used to cancel loading trailer</param>
        /// <returns>Video trailer</returns>
        public async Task<ResultContainer<Video>> GetMovieTrailerAsync(MovieJson movie, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var trailers = new ResultContainer<Video>();
            try
            {
                await Task.Run(
                    async () => trailers = (await TmdbClient.GetMovieAsync(movie.ImdbCode, MovieMethods.Videos))?.Videos,
                    ct);
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "GetMovieTrailerAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"GetMovieTrailerAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"GetMovieTrailerAsync ({movie.ImdbCode}) in {elapsedMs} milliseconds.");
            }

            return trailers;
        }

        /// <summary>
        /// Get the movie's subtitles according to a language
        /// </summary>
        /// <param name="movie">The movie of which to retrieve its subtitles</param>
        /// <param name="ct">Cancellation token</param>
        public async Task LoadSubtitlesAsync(MovieJson movie,
            CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var restClient = new RestClient(Constants.YifySubtitlesApi);
            var request = new RestRequest("/{segment}", Method.GET);
            request.AddUrlSegment("segment", movie.ImdbCode);

            try
            {
                var response = await restClient.ExecuteGetTaskAsync<SubtitlesWrapper>(request, ct);
                if (response.ErrorException != null)
                    throw response.ErrorException;

                var wrapper = response.Data;

                var subtitles = new ObservableCollection<SubtitleJson>();
                Dictionary<string, List<SubtitleJson>> movieSubtitles;
                if (wrapper.Subtitles == null)
                {
                    await Task.CompletedTask;
                    return;
                }
                if (wrapper.Subtitles.TryGetValue(movie.ImdbCode, out movieSubtitles))
                {
                    foreach (var subtitle in movieSubtitles)
                    {
                        var sub = subtitle.Value.Aggregate((sub1, sub2) => sub1.Rating > sub2.Rating ? sub1 : sub2);
                        subtitles.Add(new SubtitleJson
                        {
                            Id = sub.Id,
                            Language = new CustomLanguage
                            {
                                Culture = string.Empty,
                                EnglishName = subtitle.Key,
                                LocalizedName = string.Empty
                            },
                            Hi = sub.Hi,
                            Rating = sub.Rating,
                            Url = sub.Url
                        });
                    }
                }

                subtitles.Sort();
                movie.AvailableSubtitles = subtitles;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "LoadSubtitlesAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"LoadSubtitlesAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"LoadSubtitlesAsync ({movie.ImdbCode}) in {elapsedMs} milliseconds.");
            }
        }

        /// <summary>
        /// Download a subtitle
        /// </summary>
        /// <param name="movie">The movie of which to retrieve its subtitles</param>
        /// <param name="progress">Report the progress of the download</param>
        /// <param name="ct">Cancellation token</param>
        public async Task DownloadSubtitleAsync(MovieJson movie, IProgress<long> progress, CancellationTokenSource ct)
        {
            if (movie.SelectedSubtitle == null)
                return;

            var watch = Stopwatch.StartNew();

            var filePath = Constants.Subtitles + movie.ImdbCode + "\\" + movie.SelectedSubtitle.Language.EnglishName +
                           ".zip";

            try
            {
                var result = await
                    DownloadFileHelper.DownloadFileTaskAsync(
                        Constants.YifySubtitles + movie.SelectedSubtitle.Url, filePath, progress: progress, ct: ct);

                if (result.Item3 == null && !string.IsNullOrEmpty(result.Item2))
                {
                    using (var archive = ZipFile.OpenRead(result.Item2))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            if (entry.FullName.StartsWith("_") ||
                                !entry.FullName.EndsWith(".srt", StringComparison.OrdinalIgnoreCase)) continue;
                            var subtitlePath = Path.Combine(Constants.Subtitles + movie.ImdbCode,
                                entry.FullName);
                            if (!File.Exists(subtitlePath))
                                entry.ExtractToFile(subtitlePath);

                            movie.SelectedSubtitle.FilePath = subtitlePath;
                        }
                    }
                }
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                Logger.Debug(
                    "DownloadSubtitleAsync cancelled.");
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"DownloadSubtitleAsync: {exception.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Logger.Debug(
                    $"DownloadSubtitleAsync ({movie.ImdbCode}) in {elapsedMs} milliseconds.");
            }
        }
    }
}