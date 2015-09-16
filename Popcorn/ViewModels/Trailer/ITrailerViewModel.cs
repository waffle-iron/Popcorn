using System.Threading;
using System.Threading.Tasks;
using Popcorn.Models.Movie.Full;

namespace Popcorn.ViewModels.Trailer
{
    public interface ITrailerViewModel
    {
        /// <summary>
        /// Load movie's trailer asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <param name="ct">Cancellation token</param>
        Task LoadTrailerAsync(MovieFull movie, CancellationToken ct);

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