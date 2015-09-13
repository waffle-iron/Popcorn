using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Popcorn.Models.Genre;

namespace Popcorn.ViewModels.Genres
{
    public interface IGenresViewModel
    {
        /// <summary>
        /// Movie genres
        /// </summary>
        ObservableCollection<MovieGenre> MovieGenres { get; set; }

        /// <summary>
        /// Load genres asynchronously
        /// </summary>
        /// <returns></returns>
        Task LoadGenresAsync();

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}
