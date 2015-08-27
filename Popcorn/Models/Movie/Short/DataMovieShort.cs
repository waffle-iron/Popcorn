using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Short
{
    public class DataMovieShort : ObservableObject
    {
        private int _movieCount;

        [DeserializeAs(Name = "movie_count")]
        public int MovieCount
        {
            get { return _movieCount; }
            set { Set(() => MovieCount, ref _movieCount, value); }
        }

        private int _limit;

        [DeserializeAs(Name = "limit")]
        public int Limit
        {
            get { return _limit; }
            set { Set(() => Limit, ref _limit, value); }
        }

        private int _pageNumber;

        [DeserializeAs(Name = "page_number")]
        public int PageNumber
        {
            get { return _pageNumber; }
            set { Set(() => PageNumber, ref _pageNumber, value); }
        }

        private List<MovieShort> _movies;

        [DeserializeAs(Name = "movies")]
        public List<MovieShort> Movies
        {
            get { return _movies; }
            set { Set(() => Movies, ref _movies, value); }
        }
    }
}
