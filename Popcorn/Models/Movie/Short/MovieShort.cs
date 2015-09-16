using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Short
{
    /// <summary>
    ///     Represents partial details of a movie
    /// </summary>
    public class MovieShort : ObservableObject
    {
        private int _apiVersion;
        private string _coverImagePath;
        private string _dateUploaded;
        private int _dateUploadedUnix;
        private string _executionTime;
        private List<string> _genres;
        private bool _hasBeenSeen;
        private int _id;
        private string _imdbCode;
        private bool _isFavorite;
        private string _language;
        private string _mediumCoverImage;
        private string _mpaRating;
        private double _rating;
        private double _ratingValue;
        private int _runtime;
        private int _serverTime;
        private string _serverTimezone;
        private string _smallCoverImage;
        private string _title;
        private string _titleLong;
        private List<Torrent.Torrent> _torrents;
        private string _url;
        private int _year;

        [DeserializeAs(Name = "id")]
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        [DeserializeAs(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        [DeserializeAs(Name = "imdb_code")]
        public string ImdbCode
        {
            get { return _imdbCode; }
            set { Set(() => ImdbCode, ref _imdbCode, value); }
        }

        [DeserializeAs(Name = "title")]
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        [DeserializeAs(Name = "title_long")]
        public string TitleLong
        {
            get { return _titleLong; }
            set { Set(() => TitleLong, ref _titleLong, value); }
        }

        [DeserializeAs(Name = "year")]
        public int Year
        {
            get { return _year; }
            set { Set(() => Year, ref _year, value); }
        }

        [DeserializeAs(Name = "rating")]
        public double Rating
        {
            get { return _rating; }
            set { Set(() => Rating, ref _rating, value); }
        }

        [DeserializeAs(Name = "runtime")]
        public int Runtime
        {
            get { return _runtime; }
            set { Set(() => Runtime, ref _runtime, value); }
        }

        [DeserializeAs(Name = "genres")]
        public List<string> Genres
        {
            get { return _genres; }
            set { Set(() => Genres, ref _genres, value); }
        }

        [DeserializeAs(Name = "language")]
        public string Language
        {
            get { return _language; }
            set { Set(() => Language, ref _language, value); }
        }

        [DeserializeAs(Name = "mpa_rating")]
        public string MpaRating
        {
            get { return _mpaRating; }
            set { Set(() => MpaRating, ref _mpaRating, value); }
        }

        [DeserializeAs(Name = "small_cover_image")]
        public string SmallCoverImage
        {
            get { return _smallCoverImage; }
            set { Set(() => SmallCoverImage, ref _smallCoverImage, value); }
        }

        [DeserializeAs(Name = "medium_cover_image")]
        public string MediumCoverImage
        {
            get { return _mediumCoverImage; }
            set { Set(() => MediumCoverImage, ref _mediumCoverImage, value); }
        }

        [DeserializeAs(Name = "state")]
        public string State { get; set; }

        [DeserializeAs(Name = "torrents")]
        public List<Torrent.Torrent> Torrents
        {
            get { return _torrents; }
            set { Set(() => Torrents, ref _torrents, value); }
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

        [DeserializeAs(Name = "server_time")]
        public int ServerTime
        {
            get { return _serverTime; }
            set { Set(() => ServerTime, ref _serverTime, value); }
        }

        [DeserializeAs(Name = "server_timezone")]
        public string ServerTimezone
        {
            get { return _serverTimezone; }
            set { Set(() => ServerTimezone, ref _serverTimezone, value); }
        }

        [DeserializeAs(Name = "api_version")]
        public int ApiVersion
        {
            get { return _apiVersion; }
            set { Set(() => ApiVersion, ref _apiVersion, value); }
        }

        [DeserializeAs(Name = "execution_time")]
        public string ExecutionTime
        {
            get { return _executionTime; }
            set { Set(() => ExecutionTime, ref _executionTime, value); }
        }

        /// <summary>
        ///     Movie rating
        /// </summary>
        public double RatingValue
        {
            get { return _ratingValue; }
            set { Set(() => RatingValue, ref _ratingValue, value); }
        }

        /// <summary>
        ///     Local path of the downloaded movie's cover image
        /// </summary>
        public string CoverImagePath
        {
            get { return _coverImagePath; }
            set { Set(() => CoverImagePath, ref _coverImagePath, value); }
        }

        /// <summary>
        ///     Indicate if movie is favorite
        /// </summary>
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { Set(() => IsFavorite, ref _isFavorite, value); }
        }

        /// <summary>
        ///     Indicate if movie has been seen by the user
        /// </summary>
        public bool HasBeenSeen
        {
            get { return _hasBeenSeen; }
            set { Set(() => HasBeenSeen, ref _hasBeenSeen, value); }
        }
    }
}