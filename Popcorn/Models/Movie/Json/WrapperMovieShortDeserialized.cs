using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class WrapperMovieShortDeserialized
    {
        [DeserializeAs(Name = "status")]
        public string Status { get; set; }

        [DeserializeAs(Name = "status_message")]
        public string StatusMessage { get; set; }

        [DeserializeAs(Name = "data")]
        public DataMovieShortDeserialized Data { get; set; }

        [DeserializeAs(Name = "@meta")]
        public MetaDeserialized Meta { get; set; }
    }
}
