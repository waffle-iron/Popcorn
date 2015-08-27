using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Popcorn.Models.Cast;
using Popcorn.Models.Torrent.Deserialized;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class MovieFullDeserialized : ObservableObject
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

        [DeserializeAs(Name = "download_count")]
        public string DownloadCount { get; set; }

        [DeserializeAs(Name = "like_count")]
        public string LikeCount { get; set; }

        [DeserializeAs(Name = "rt_critics_score")]
        public string RtCrtiticsScore { get; set; }

        [DeserializeAs(Name = "rt_critics_rating")]
        public string RtCriticsRating { get; set; }

        [DeserializeAs(Name = "rt_audience_score")]
        public string RtAudienceScore { get; set; }

        [DeserializeAs(Name = "rt_audience_rating")]
        public string RtAudienceRating { get; set; }

        [DeserializeAs(Name = "description_intro")]
        public string DescriptionIntro { get; set; }

        [DeserializeAs(Name = "description_full")]
        public string DescriptionFull { get; set; }

        [DeserializeAs(Name = "yt_trailer_code")]
        public string YtTrailerCode { get; set; }

        [DeserializeAs(Name = "images")]
        public MovieImagesDeserialized Images { get; set; }

        [DeserializeAs(Name = "directors")]
        public List<Director> Directors { get; set; }

        [DeserializeAs(Name = "actors")]
        public List<Actor> Actors { get; set; }

        [DeserializeAs(Name = "torrents")]
        public List<TorrentDeserialized> Torrents { get; set; }

        [DeserializeAs(Name = "date_uploaded")]
        public string DateUploaded { get; set; }

        [DeserializeAs(Name = "date_uploaded_unix")]
        public int DateUploadedUnix { get; set; }
    }
}
