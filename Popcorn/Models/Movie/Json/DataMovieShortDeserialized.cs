using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class DataMovieShortDeserialized 
    {
        [DeserializeAs(Name = "movie_count")]
        public int MovieCount { get; set; }

        [DeserializeAs(Name = "limit")]
        public int Limit { get; set; }

        [DeserializeAs(Name = "page_nombre")]
        public int PageNumber { get; set; }

        [DeserializeAs(Name = "movies")]
        public List<MovieShortDeserialized> Movies { get; set; }
    }
}
