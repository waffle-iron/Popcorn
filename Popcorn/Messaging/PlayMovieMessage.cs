using GalaSoft.MvvmLight.Messaging;
using Popcorn.Models.Movie.Full;

namespace Popcorn.Messaging
{
    /// <summary>
    ///     Used to play a movie
    /// </summary>
    public class PlayMovieMessage : MessageBase
    {
        /// <summary>
        ///     The buffered movie
        /// </summary>
        public readonly MovieFull Movie;

        /// <summary>
        ///     Initialize a new instance of PlayMovieMessage class
        /// </summary>
        /// <param name="movie">The movie</param>
        public PlayMovieMessage(MovieFull movie)
        {
            Movie = movie;
        }
    }
}