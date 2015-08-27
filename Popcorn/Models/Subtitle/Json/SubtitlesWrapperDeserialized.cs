using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Popcorn.Models.Subtitle.Json
{
    public class SubtitlesWrapperDeserialized
    {
        [DeserializeAs(Name = "success")]
        public bool Success { get; set; }

        [DeserializeAs(Name = "lastModified")]
        public int LastModified { get; set; }

        [DeserializeAs(Name = "subtitles")]
        public int SubtitlesCount { get; set; }

        [DeserializeAs(Name = "subs")]
        public Dictionary<string, Dictionary<string, List<SubtitleDeserialized>>> Subtitles{ get; set; }
    }
}
