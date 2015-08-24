using GalaSoft.MvvmLight;
using TMDbLib.Objects.General;

namespace Popcorn.Models.Movie
{
    public class MovieGenre : ObservableObject
    {
        #region Property -> EnglishName

        private string _englishName;

        /// <summary>
        /// Genre's english name
        /// </summary>
        public string EnglishName
        {
            get { return _englishName; }
            set { Set(() => EnglishName, ref _englishName, value); }
        }

        #endregion

        #region Property -> TmdbGenre

        private Genre _tmdbGenre;

        /// <summary>
        /// Tmdb genre
        /// </summary>
        public Genre TmdbGenre
        {
            get { return _tmdbGenre; }
            set { Set(() => TmdbGenre, ref _tmdbGenre, value); }
        }

        #endregion
    }
}
