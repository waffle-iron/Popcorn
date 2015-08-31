using System.Collections.Generic;
using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Short
{
    /// <summary>
    /// Represents partial details of a movie
    /// </summary>
    public class MovieShort : ObservableObject
    {
        #region Properties

        private int _id;

        [DeserializeAs(Name = "id")]
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        private string _url;

        [DeserializeAs(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        private string _imdbCode;

        [DeserializeAs(Name = "imdb_code")]
        public string ImdbCode
        {
            get { return _imdbCode; }
            set { Set(() => ImdbCode, ref _imdbCode, value); }
        }

        private string _title;

        [DeserializeAs(Name = "title")]
        public string Title
        {
            get { return _title; }
            set { Set(() => Title, ref _title, value); }
        }

        private string _titleLong;

        [DeserializeAs(Name = "title_long")]
        public string TitleLong
        {
            get { return _titleLong; }
            set { Set(() => TitleLong, ref _titleLong, value); }
        }

        private int _year;

        [DeserializeAs(Name = "year")]
        public int Year
        {
            get { return _year; }
            set { Set(() => Year, ref _year, value); }
        }

        private double _rating;

        [DeserializeAs(Name = "rating")]
        public double Rating
        {
            get { return _rating; }
            set { Set(() => Rating, ref _rating, value); }
        }

        private int _runtime;

        [DeserializeAs(Name = "runtime")]
        public int Runtime
        {
            get { return _runtime; }
            set { Set(() => Runtime, ref _runtime, value); }
        }

        private List<string> _genres;

        [DeserializeAs(Name = "genres")]
        public List<string> Genres
        {
            get { return _genres; }
            set { Set(() => Genres, ref _genres, value); }
        }

        private string _language;

        [DeserializeAs(Name = "language")]
        public string Language
        {
            get { return _language; }
            set { Set(() => Language, ref _language, value); }
        }

        private string _mpaRating;

        [DeserializeAs(Name = "mpa_rating")]
        public string MpaRating
        {
            get { return _mpaRating; }
            set { Set(() => MpaRating, ref _mpaRating, value); }
        }

        private string _smallCoverImage;

        [DeserializeAs(Name = "small_cover_image")]
        public string SmallCoverImage
        {
            get { return _smallCoverImage; }
            set { Set(() => SmallCoverImage, ref _smallCoverImage, value); }
        }

        private string _mediumCoverImage;

        [DeserializeAs(Name = "medium_cover_image")]
        public string MediumCoverImage
        {
            get { return _mediumCoverImage; }
            set { Set(() => MediumCoverImage, ref _mediumCoverImage, value); }
        }

        [DeserializeAs(Name = "state")]
        public string State { get; set; }

        private List<Torrent.Torrent> _torrents;

        [DeserializeAs(Name = "torrents")]
        public List<Torrent.Torrent> Torrents
        {
            get { return _torrents; }
            set { Set(() => Torrents, ref _torrents, value); }
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

        private int _serverTime;

        [DeserializeAs(Name = "server_time")]
        public int ServerTime
        {
            get { return _serverTime; }
            set { Set(() => ServerTime, ref _serverTime, value); }
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

        #region Property -> RatingValue

        private double _ratingValue;

        /// <summary>
        /// Movie rating
        /// </summary>
        public double RatingValue
        {
            get { return _ratingValue; }
            set { Set(() => RatingValue, ref _ratingValue, value); }
        }
        
        #endregion

        #region Property -> CoverImagePath

        private string _coverImagePath = string.Empty;

        /// <summary>
        /// Local path of the downloaded movie's cover image
        /// </summary>
        public string CoverImagePath
        {
            get { return _coverImagePath; }
            set { Set(() => CoverImagePath, ref _coverImagePath, value); }
        }

        #endregion

        #endregion

        #region Property -> IsFavorite

        private bool _isFavorite;

        /// <summary>
        /// Indicate if movie is favorite
        /// </summary>
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { Set(() => IsFavorite, ref _isFavorite, value); }
        }

        #endregion

        #region Property -> HasBeenSeen

        private bool _hasBeenSeen;

        /// <summary>
        /// Indicate if movie has been seen by the user
        /// </summary>
        public bool HasBeenSeen
        {
            get { return _hasBeenSeen; }
            set { Set(() => HasBeenSeen, ref _hasBeenSeen, value); }
        }

        #endregion
    }
}