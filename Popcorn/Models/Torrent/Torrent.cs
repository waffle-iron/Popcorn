using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Torrent
{
    public class Torrent : ObservableObject
    {
        private string _url;

        [DeserializeAs(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        private string _hash;

        [DeserializeAs(Name = "hash")]
        public string Hash
        {
            get { return _hash; }
            set { Set(() => Hash, ref _hash, value); }
        }

        private string _quality;

        [DeserializeAs(Name = "quality")]
        public string Quality
        {
            get { return _quality; }
            set { Set(() => Quality, ref _quality, value); }
        }

        private string _framerate;

        [DeserializeAs(Name = "framerate")]
        public string Framerate
        {
            get { return _framerate; }
            set { Set(() => Framerate, ref _framerate, value); }
        }

        private string _resolution;

        [DeserializeAs(Name = "resolution")]
        public string Resolution
        {
            get { return _resolution; }
            set { Set(() => Resolution, ref _resolution, value); }
        }

        private int _seeds;

        [DeserializeAs(Name = "seeds")]
        public int Seeds
        {
            get { return _seeds; }
            set { Set(() => Seeds, ref _seeds, value); }
        }

        private int _peers;

        [DeserializeAs(Name = "peers")]
        public int Peers
        {
            get { return _peers; }
            set { Set(() => Peers, ref _peers, value); }
        }

        private string _size;

        [DeserializeAs(Name = "size")]
        public string Size
        {
            get { return _size; }
            set { Set(() => Size, ref _size, value); }
        }

        private long _sizeBytes;

        [DeserializeAs(Name = "size_bytes")]
        public long SizeBytes
        {
            get { return _sizeBytes; }
            set { Set(() => SizeBytes, ref _sizeBytes, value); }
        }

        private string _dateUploaded;

        [DeserializeAs(Name = "date_uploaded")]
        public string DateUploaded
        {
            get { return _dateUploaded; }
            set { Set(() => DateUploaded, ref _dateUploaded, value); }
        }

        private int _dateUploadedUnix;

        [DeserializeAs(Name = "date_uploaded_unix")]
        public int DateUploadedUnix
        {
            get { return _dateUploadedUnix; }
            set { Set(() => DateUploadedUnix, ref _dateUploadedUnix, value); }
        }
    }
}
