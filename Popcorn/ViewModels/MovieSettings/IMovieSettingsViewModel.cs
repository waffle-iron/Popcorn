using Popcorn.Models.Movie.Full;

namespace Popcorn.ViewModels.MovieSettings
{
    public interface IMovieSettingsViewModel
    {
        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        void LoadMovie(MovieFull movie);

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}