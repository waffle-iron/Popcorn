using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Full
{
    public class WrapperMovieFull : ObservableObject
    {
        private Meta _meta;

        private MovieFull _movie;
        private string _status;

        private string _statusMessage;

        [DeserializeAs(Name = "status")]
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        [DeserializeAs(Name = "status_message")]
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(() => StatusMessage, ref _statusMessage, value); }
        }

        [DeserializeAs(Name = "data")]
        public MovieFull Movie
        {
            get { return _movie; }
            set { Set(() => Movie, ref _movie, value); }
        }

        [DeserializeAs(Name = "@meta")]
        public Meta Meta
        {
            get { return _meta; }
            set { Set(() => Meta, ref _meta, value); }
        }
    }
}