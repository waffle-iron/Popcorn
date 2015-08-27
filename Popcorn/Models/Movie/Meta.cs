using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie
{
    public class Meta : ObservableObject
    {
        private int _serverTimer;

        [DeserializeAs(Name = "server_time")]
        public int ServerTime
        {
            get { return _serverTimer; }
            set { Set(() => ServerTime, ref _serverTimer, value); }
        }

        private string _serverTimezone;

        [DeserializeAs(Name = "server_timezone")]
        public string ServerTimezone
        {
            get { return _serverTimezone; }
            set { Set(() => ServerTimezone, ref _serverTimezone, value); }
        }

        private int _apiVersion;

        [DeserializeAs(Name = "api_version")]
        public int ApiVersion
        {
            get { return _apiVersion; }
            set { Set(() => ApiVersion, ref _apiVersion, value); }
        }

        private string _executionTime;

        [DeserializeAs(Name = "execution_time")]
        public string ExecutionTime
        {
            get { return _executionTime; }
            set { Set(() => ExecutionTime, ref _executionTime, value); }
        }
    }
}
