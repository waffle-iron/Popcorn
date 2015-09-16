using Popcorn.Models.Movie.Full;

namespace Popcorn.ViewModels.Players.Movie
{
    public interface IMoviePlayerViewModel
    {
        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">Movie to load</param>
        void LoadMovie(MovieFull movie);

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}