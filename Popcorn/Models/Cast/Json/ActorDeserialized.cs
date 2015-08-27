using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Cast.Json
{
    public class ActorDeserialized : ObservableObject
    {
        private string _name;
        [DeserializeAs(Name = "name")]
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        private string _characterName;
        [DeserializeAs(Name = "character_name")]
        public string CharacterName
        {
            get { return _characterName; }
            set { Set(() => CharacterName, ref _characterName, value); }
        }

        private string _smallImage;
        [DeserializeAs(Name = "small_image")]
        public string SmallImage 
        {
            get { return _smallImage; }
            set { Set(() => SmallImage, ref _smallImage, value); }
        }

        private string _mediumImage;
        [DeserializeAs(Name = "medium_image")]
        public string MediumImage 
        {
            get { return _mediumImage; }
            set { Set(() => MediumImage, ref _mediumImage, value); }
        }
    }
}
