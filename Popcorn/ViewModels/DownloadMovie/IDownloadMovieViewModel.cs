using Popcorn.Models.Movie.Full;

namespace Popcorn.ViewModels.DownloadMovie
{
    public interface IDownloadMovieViewModel
    {
        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        void LoadMovie(MovieFull movie);

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