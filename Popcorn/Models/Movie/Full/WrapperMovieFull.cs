using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Full
{
    public class WrapperMovieFull : ObservableObject
    {
        private string _status;

        [DeserializeAs(Name = "status")]
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        private string _statusMessage;

        [DeserializeAs(Name = "status_message")]
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(() => StatusMessage, ref _statusMessage, value); }

        }

        private MovieFull _movie;

        [DeserializeAs(Name = "data")]
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        private Meta _meta;

        [DeserializeAs(Name = "@meta")]
        public Meta Meta
        {
            get { return _meta; }
            set { Set(() => Meta, ref _meta, value); }
        }
    }
}
