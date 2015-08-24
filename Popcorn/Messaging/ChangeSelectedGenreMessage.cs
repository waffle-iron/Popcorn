using GalaSoft.MvvmLight.Messaging;
using Popcorn.Models.Movie;

namespace Popcorn.Messaging
{
    /// <summary>
    /// Used to broadcast a changed genre message
    /// </summary>
    public class ChangeSelectedGenreMessage : MessageBase
    {
        #region Properties

        #region Property -> Genre

        /// <summary>
        /// Movie
        /// </summary>
        public readonly MovieGenre Genre;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// ChangeSelectedGenreMessage
        /// </summary>
        /// <param name="genre">The genre</param>
        public ChangeSelectedGenreMessage(MovieGenre genre)
        {
            Genre = genre;
        }

        #endregion
    }
}