using Popcorn.Models.Movie;

namespace Popcorn.ViewModels.MovieSettings
{
    public interface IMovieSettingsViewModel
    {
        /// <summary>
        /// Load a movie
        /// </summary>
        /// <param name="movie">The movie to load</param>
        void LoadMovie(MovieJson movie);

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}