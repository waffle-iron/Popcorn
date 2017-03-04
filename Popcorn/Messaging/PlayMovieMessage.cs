using GalaSoft.MvvmLight.Messaging;
using Popcorn.Models.Movie;

namespace Popcorn.Messaging
{
    /// <summary>
    /// Used to play a movie
    /// </summary>
    public class PlayMovieMessage : MessageBase
    {
        /// <summary>
        /// The buffered movie
        /// </summary>
        public readonly MovieJson Movie;

        /// <summary>
        /// Initialize a new instance of PlayMovieMessage class
        /// </summary>
        /// <param name="movie">The movie</param>
        public PlayMovieMessage(MovieJson movie)
        {
            Movie = movie;
        }
    }
}