using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Torrent
{
    public class Torrent : ObservableObject
    {
        private string _dateUploaded;

        private int _dateUploadedUnix;

        private string _framerate;

        private string _hash;

        private int _peers;

        private string _quality;

        private string _resolution;

        private int _seeds;

        private string _size;

        private long _sizeBytes;
        private string _url;

        [DeserializeAs(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        [DeserializeAs(Name = "hash")]
        public string Hash
        {
            get { return _hash; }
            set { Set(() => Hash, ref _hash, value); }
        }

        [DeserializeAs(Name = "quality")]
        public string Quality
        {
            get { return _quality; }
            set { Set(() => Quality, ref _quality, value); }
        }

        [DeserializeAs(Name = "framerate")]
        public string Framerate
        {
            get { return _framerate; }
            set { Set(() => Framerate, ref _framerate, value); }
        }

        [DeserializeAs(Name = "resolution")]
        public string Resolution
        {
            get { return _resolution; }
            set { Set(() => Resolution, ref _resolution, value); }
        }

        [DeserializeAs(Name = "seeds")]
        public int Seeds
        {
            get { return _seeds; }
            set { Set(() => Seeds, ref _seeds, value); }
        }

        [DeserializeAs(Name = "peers")]
        public int Peers
        {
            get { return _peers; }
            set { Set(() => Peers, ref _peers, value); }
        }

        [DeserializeAs(Name = "size")]
        public string Size
        {
            get { return _size; }
            set { Set(() => Size, ref _size, value); }
        }

        [DeserializeAs(Name = "size_bytes")]
        public long SizeBytes
        {
            get { return _sizeBytes; }
            set { Set(() => SizeBytes, ref _sizeBytes, value); }
        }

        [DeserializeAs(Name = "date_uploaded")]
        public string DateUploaded
        {
            get { return _dateUploaded; }
            set { Set(() => DateUploaded, ref _dateUploaded, value); }
        }

        [DeserializeAs(Name = "date_uploaded_unix")]
        public int DateUploadedUnix
        {
            get { return _dateUploadedUnix; }
            set { Set(() => DateUploadedUnix, ref _dateUploadedUnix, value); }
        }
    }
}