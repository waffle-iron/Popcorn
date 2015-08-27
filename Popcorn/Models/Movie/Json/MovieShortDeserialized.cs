using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Popcorn.Models.Torrent.Deserialized;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class MovieShortDeserialized : ObservableObject
    {
        [DeserializeAs(Name = "id")]
        public int Id { get; set; }

        [DeserializeAs(Name = "url")]
        public string Url { get; set; }

        [DeserializeAs(Name = "imdb_code")]
        public string ImdbCode { get; set; }

        [DeserializeAs(Name = "title")]
        public string Title { get; set; }

        [DeserializeAs(Name = "title_long")]
        public string TitleLong { get; set; }

        [DeserializeAs(Name = "year")]
        public int Year { get; set; }

        [DeserializeAs(Name = "rating")]
        public double Rating { get; set; }

        [DeserializeAs(Name = "runtime")]
        public int Runtime { get; set; }

        [DeserializeAs(Name = "genres")]
        public List<string> Genres { get; set; }

        [DeserializeAs(Name = "language")]
        public string Language { get; set; }

        [DeserializeAs(Name = "mpa_rating")]
        public string MpaRating { get; set; }

        [DeserializeAs(Name = "small_cover_image")]
        public string SmallCoverImage { get; set; }

        [DeserializeAs(Name = "medium_cover_image")]
        public string MediumCoverImage { get; set; }

        [DeserializeAs(Name = "state")]
        public string State { get; set; }

        [DeserializeAs(Name = "torrents")]
        public List<TorrentDeserialized> Torrents { get; set; }

        [DeserializeAs(Name = "date_uploaded")]
        public string DateUploaded { get; set; }

        [DeserializeAs(Name = "date_uploaded_unix")]
        public int DateUploadedUnix { get; set; }

        [DeserializeAs(Name = "server_time")]
        public int ServerTime { get; set; }

        [DeserializeAs(Name = "server_timezone")]
        public string ServerTimezone { get; set; }

        [DeserializeAs(Name = "api_version")]
        public int ApiVersion { get; set; }

        [DeserializeAs(Name = "execution_time")]
        public string ExecutionTime { get; set; }
    }
}
