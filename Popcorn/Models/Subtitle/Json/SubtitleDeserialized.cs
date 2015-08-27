using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Subtitle.Json
{
    public class SubtitleDeserialized : ObservableObject
    {
        [DeserializeAs(Name = "id")]
        public int Id { get; set; }

        [DeserializeAs(Name = "hi")]
        public int Hi { get; set; }

        [DeserializeAs(Name = "rating")]
        public int Rating { get; set; }

        [DeserializeAs(Name = "url")]
        public string Url { get; set; }
    }
}
