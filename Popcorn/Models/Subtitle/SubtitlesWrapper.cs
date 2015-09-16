using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Subtitle
{
    public class SubtitlesWrapper : ObservableObject
    {
        private int _lastModified;
        private Dictionary<string, Dictionary<string, List<Subtitle>>> _subtitles;
        private int _subtitlesCount;
        private bool _success;

        [DeserializeAs(Name = "success")]
        public bool Success
        {
            get { return _success; }
            set { Set(() => Success, ref _success, value); }
        }

        [DeserializeAs(Name = "lastModified")]
        public int LastModified
        {
            get { return _lastModified; }
            set { Set(() => LastModified, ref _lastModified, value); }
        }

        [DeserializeAs(Name = "subtitles")]
        public int SubtitlesCount
        {
            get { return _subtitlesCount; }
            set { Set(() => SubtitlesCount, ref _subtitlesCount, value); }
        }

        [DeserializeAs(Name = "subs")]
        public Dictionary<string, Dictionary<string, List<Subtitle>>> Subtitles
        {
            get { return _subtitles; }
            set { Set(() => Subtitles, ref _subtitles, value); }
        }
    }
}