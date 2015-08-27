using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Short
{
    public class WrapperMovieShort : ObservableObject
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

        private DataMovieShort _data;

        [DeserializeAs(Name = "data")]
        public DataMovieShort Data
        {
            get { return _data; }
            set { Set(() => Data, ref _data, value); }
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
