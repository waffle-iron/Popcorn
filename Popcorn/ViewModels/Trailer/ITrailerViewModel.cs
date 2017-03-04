using System.Threading;
using System.Threading.Tasks;
using Popcorn.Models.Movie;

namespace Popcorn.ViewModels.Trailer
{
    public interface ITrailerViewModel
    {
        /// <summary>
        /// Load movie's trailer asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Cancellation token</param>
        Task LoadTrailerAsync(MovieJson movie, CancellationToken ct);

        /// <summary>
        /// Unload the trailer
        /// </summary>
        void UnLoadTrailer();

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}