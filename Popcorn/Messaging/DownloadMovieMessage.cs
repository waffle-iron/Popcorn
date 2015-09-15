using GalaSoft.MvvmLight.Messaging;
using Popcorn.Models.Movie.Full;

namespace Popcorn.Messaging
{
    /// <summary>
    ///     Used to broadcast a downloading movie message action
    /// </summary>
    public class DownloadMovieMessage : MessageBase
    {
        /// <summary>
        ///     Movie
        /// </summary>
        public readonly MovieFull Movie;

        /// <summary>
        ///     DownloadMovieMessage
        /// </summary>
        /// <param name="movie">The movie to download</param>
        public DownloadMovieMessage(MovieFull movie)
        {
            Movie = movie;
        }
    }
}