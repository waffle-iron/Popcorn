using GalaSoft.MvvmLight;

namespace Popcorn.Models.Genre
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

        private TMDbLib.Objects.General.Genre _tmdbGenre;

        /// <summary>
        /// Tmdb genre
        /// </summary>
        public TMDbLib.Objects.General.Genre TmdbGenre
        {
            get { return _tmdbGenre; }
            set { Set(() => TmdbGenre, ref _tmdbGenre, value); }
        }

        #endregion
    }
}
