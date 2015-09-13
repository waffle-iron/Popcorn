using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Popcorn.Models.Genre;
using Popcorn.Models.Localization;
using TMDbLib.Objects.General;
using Popcorn.Models.Movie.Full;
using Popcorn.Models.Movie.Short;

namespace Popcorn.Services.Movie
{
    public interface IMovieService
    {
        /// <summary>
        /// Change the culture of TMDb
        /// </summary>
        /// <param name="language">Language to set</param>
        void ChangeTmdbLanguage(ILanguage language);

        /// <summary>
        /// Get all movie's genres
        /// </summary>
        /// <param name="ct">Used to cancel loading genres</param>
        /// <returns>Genres</returns>
        Task<List<MovieGenre>> GetGenresAsync(CancellationToken ct);

        /// <summary>
        /// Get popular movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Popular movies and the number of movies found</returns>
        Task<Tuple<IEnumerable<MovieShort>, int>> GetPopularMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            MovieGenre genre = null);

        /// <summary>
        /// Get greatest movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Top rated movies and the number of movies found</returns>
        Task<Tuple<IEnumerable<MovieShort>, int>> GetGreatestMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            MovieGenre genre = null);

        /// <summary>
        /// Get recent movies by page
        /// </summary>
        /// <param name="page">Page to return</param>
        /// <param name="limit">The maximum number of movies to return</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="genre">The genre to filter</param>
        /// <param name="ratingFilter">Used to filter by rating</param>
        /// <returns>Recent movies and the number of movies found</returns>
        Task<Tuple<IEnumerable<MovieShort>, int>> GetRecentMoviesAsync(int page,
            int limit,
            double ratingFilter,
            CancellationToken ct,
            MovieGenre genre = null);

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
        Task<Tuple<IEnumerable<MovieShort>, int>> SearchMoviesAsync(string criteria,
            int page,
            int limit,
            MovieGenre genre,
            double ratingFilter,
            CancellationToken ct);


        /// <summary>
        /// Get TMDb movie informations
        /// </summary>
        /// <param name="movieToLoad">Movie to load</param>
        /// <param name="ct">Used to cancel loading</param>
        /// <returns>Movie's full details</returns>
        Task<MovieFull> GetMovieFullDetailsAsync(MovieShort movieToLoad, CancellationToken ct);

        /// <summary>
        /// Translate movie informations (title, description, ...)
        /// </summary>
        /// <param name="movieToTranslate">Movie to translate</param>
        /// <param name="ct">Used to cancel translation</param>
        /// <returns>Task</returns>
        Task TranslateMovieShortAsync(MovieShort movieToTranslate, CancellationToken ct);

        /// <summary>
        /// Translate movie informations (title, description, ...)
        /// </summary>
        /// <param name="movieToTranslate">Movie to translate</param>
        /// <param name="ct">Used to cancel translation</param>
        /// <returns>Task</returns>
        Task TranslateMovieFullAsync(MovieFull movieToTranslate, CancellationToken ct);

        /// <summary>
        /// Get the link to the youtube trailer of a movie
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Used to cancel loading trailer</param>
        /// <returns>Video trailer</returns>
        Task<ResultContainer<Video>> GetMovieTrailerAsync(MovieFull movie, CancellationToken ct);

        /// <summary>
        /// Get the movie's subtitles according to a language
        /// </summary>
        /// <param name="movie">The movie of which to retrieve its subtitles</param>
        /// <param name="ct">Cancellation token</param>
        Task LoadSubtitlesAsync(MovieFull movie,
            CancellationToken ct);

        /// <summary>
        /// Download a subtitle
        /// </summary>
        /// <param name="movie">The movie of which to retrieve its subtitles</param>
        /// <param name="progress">Report the progress of the download</param>
        /// <param name="ct">Cancellation token</param>
        Task DownloadSubtitleAsync(MovieFull movie, IProgress<long> progress, CancellationTokenSource ct);

        /// <summary>
        /// Download the movie's background image
        /// </summary>
        /// <param name="movie">The movie to process</param>
        /// <param name="ct">Used to cancel downloading background image</param>
        Task DownloadBackgroundImageAsync(MovieFull movie, CancellationTokenSource ct);

        /// <summary>
        /// Download cover image for each of the movies provided
        /// </summary>
        /// <param name="movies">The movies to process</param>
        /// <param name="ct">Used to cancel task</param>
        Task DownloadCoverImageAsync(IEnumerable<MovieShort> movies, CancellationTokenSource ct);

        /// <summary>
        /// Download the movie's poster image
        /// </summary>
        /// <param name="movie">The movie to process</param>
        /// <param name="ct">Used to cancel downloading poster image</param>
        Task DownloadPosterImageAsync(MovieFull movie, CancellationTokenSource ct);

        /// <summary>
        /// Download directors' image for a movie
        /// </summary>
        /// <param name="movie">The movie to process</param>
        /// <param name="ct">Used to cancel downloading director image</param>
        Task DownloadDirectorImageAsync(MovieFull movie, CancellationTokenSource ct);

        /// <summary>
        /// Download actors' image for a movie
        /// </summary>
        /// <param name="movie">The movie to process</param>
        /// <param name="ct">Used to cancel downloading actor image</param>
        Task DownloadActorImageAsync(MovieFull movie, CancellationTokenSource ct);
    }
}
