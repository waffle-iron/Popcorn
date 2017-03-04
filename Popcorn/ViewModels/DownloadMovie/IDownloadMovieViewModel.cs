using Popcorn.Models.Movie;

namespace Popcorn.ViewModels.DownloadMovie
{
    public interface IDownloadMovieViewModel
    {
        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        void LoadMovie(MovieJson movie);

        /// <summary>
        /// Stop downloading a movie
        /// </summary>
        void StopDownloadingMovie();

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}