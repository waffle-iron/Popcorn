using RestSharp.Deserializers;

namespace Popcorn.Models.Torrent.Deserialized
{
    public class TorrentDeserialized
    {
        [DeserializeAs(Name = "url")]
        public string Url { get; set; }

        [DeserializeAs(Name = "hash")]
        public string Hash { get; set; }

        [DeserializeAs(Name = "quality")]
        public string Quality { get; set; }

        [DeserializeAs(Name = "framerate")]
        public string Framerate { get; set; }

        [DeserializeAs(Name = "resolution")]
        public string Resolution { get; set; }

        [DeserializeAs(Name = "seeds")]
        public int Seeds { get; set; }

        [DeserializeAs(Name = "peers")]
        public int Peers { get; set; }

        [DeserializeAs(Name = "size")]
        public string Size { get; set; }

        [DeserializeAs(Name = "size_bytes")]
        public long SizeBytes { get; set; }

        [DeserializeAs(Name = "date_uploaded")]
        public string DateUploaded { get; set; }

        [DeserializeAs(Name = "date_uploaded_unix")]
        public int DateUploadedMix { get; set; }
    }
}
