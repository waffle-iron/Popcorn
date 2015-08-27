using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Cast.Json
{
    public class DirectorDeserialized : ObservableObject
    {
    	private string _name;
        [DeserializeAs(Name = "name")]
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
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
