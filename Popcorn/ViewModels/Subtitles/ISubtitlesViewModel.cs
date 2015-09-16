using System.Threading.Tasks;
using Popcorn.Models.Movie.Full;

namespace Popcorn.ViewModels.Subtitles
{
    public interface ISubtitlesViewModel
    {
        /// <summary>
        /// Specify if movie's subtitles are enabled
        /// </summary>
        bool EnabledSubtitles { get; set; }

        /// <summary>
        /// Load the movie's subtitles asynchronously
        /// </summary>
        /// <param name="movie">The movie</param>
        Task LoadSubtitlesAsync(MovieFull movie);

        /// <summary>
        /// Stop downloading subtitles and clear movie
        /// </summary>
        void ClearSubtitles();

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}