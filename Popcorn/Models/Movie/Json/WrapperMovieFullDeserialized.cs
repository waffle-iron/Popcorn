using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class WrapperMovieFullDeserialized
    {
        [DeserializeAs(Name = "status")]
        public string Status { get; set; }

        [DeserializeAs(Name = "status_message")]
        public string StatusMessage { get; set; }

        [DeserializeAs(Name = "data")]
        public MovieFullDeserialized Movie { get; set; }

        [DeserializeAs(Name = "@meta")]
        public MetaDeserialized Meta { get; set; }
    }
}
