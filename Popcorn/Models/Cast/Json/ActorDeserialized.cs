using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Cast.Json
{
    public class ActorDeserialized : ObservableObject
    {
        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "character_name")]
        public string CharacterName { get; set; }

        [DeserializeAs(Name = "small_image")]
        public string SmallImage { get; set; }

        [DeserializeAs(Name = "medium_image")]
        public string MediumImage { get; set; }
    }
}
