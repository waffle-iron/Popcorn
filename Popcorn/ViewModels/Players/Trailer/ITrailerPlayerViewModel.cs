namespace Popcorn.ViewModels.Players.Trailer
{
    public interface ITrailerPlayerViewModel
    {
        /// <summary>
        /// Load a trailer
        /// </summary>
        /// <param name="trailer">Trailer to load</param>
        void LoadTrailer(Models.Trailer.Trailer trailer);

        /// <summary>
        /// Cleanup resources
        /// </summary>
        void Cleanup();
    }
}