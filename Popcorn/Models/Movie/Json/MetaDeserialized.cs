using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class MetaDeserialized
    {
        [DeserializeAs(Name = "server_time")]
        public int ServerTime { get; set; }

        [DeserializeAs(Name = "server_timezone")]
        public string ServerTimezone { get; set; }

        [DeserializeAs(Name = "api_version")]
        public int ApiVersion { get; set; }

        [DeserializeAs(Name = "execution_time")]
        public string ExecutionTime { get; set; }
    }
}
