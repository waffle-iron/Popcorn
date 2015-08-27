using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Popcorn.Models.Cast;
using Popcorn.Models.Images;
using Popcorn.Models.Torrent.Deserialized;
using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Full
{
    /// <summary>
    /// Represents all of the movie's details
    /// </summary>
    public class MovieFull : ObservableObject
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

        private string _downloadCount;

        [DeserializeAs(Name = "download_count")]
        public string DownloadCount
        {
            get { return _downloadCount; }
            set { Set(() => DownloadCount, ref _downloadCount, value); }
        }

        private string _likeCount;

        [DeserializeAs(Name = "like_count")]
        public string LikeCount
        {
            get { return _likeCount; }
            set { Set(() => LikeCount, ref _likeCount, value); }
        }

        private string _rtCriticsScore;

        [DeserializeAs(Name = "rt_critics_score")]
        public string RtCrtiticsScore
        {
            get { return _rtCriticsScore; }
            set { Set(() => RtCrtiticsScore, ref _rtCriticsScore, value); }
        }

        private string _rtCriticsRating;

        [DeserializeAs(Name = "rt_critics_rating")]
        public string RtCriticsRating
        {
            get { return _rtCriticsRating; }
            set { Set(() => RtCriticsRating, ref _rtCriticsRating, value); }
        }

        private string _rtAudienceScore;

        [DeserializeAs(Name = "rt_audience_score")]
        public string RtAudienceScore
        {
            get { return _rtAudienceScore; }
            set { Set(() => RtAudienceScore, ref _rtAudienceScore, value); }
        }

        private string _rtAudienceRating;

        [DeserializeAs(Name = "rt_audience_rating")]
        public string RtAudienceRating
        {
            get { return _rtAudienceRating; }
            set { Set(() => RtAudienceRating, ref _rtAudienceRating, value); }
        }

        private string _descriptionIntro;

        [DeserializeAs(Name = "description_intro")]
        public string DescriptionIntro
        {
            get { return _descriptionIntro; }
            set { Set(() => DescriptionIntro, ref _descriptionIntro, value); }
        }

        private string _descriptionFull;

        [DeserializeAs(Name = "description_full")]
        public string DescriptionFull
        {
            get { return _descriptionFull; }
            set { Set(() => DescriptionFull, ref _descriptionFull, value); }
        }

        private string _ytTrailerCode;

        [DeserializeAs(Name = "yt_trailer_code")]
        public string YtTrailerCode
        {
            get { return _ytTrailerCode; }
            set { Set(() => YtTrailerCode, ref _ytTrailerCode, value); }
        }

        private MovieImages _images;

        [DeserializeAs(Name = "images")]
        public MovieImages Images
        {
            get { return _images; }
            set { Set(() => Images, ref _images, value); }
        }

        private List<Director> _directors;

        [DeserializeAs(Name = "directors")]
        public List<Director> Directors
        {
            get { return _directors; }
            set { Set(() => Directors, ref _directors, value); }
        }

        private List<Actor> _actors;

        [DeserializeAs(Name = "actors")]
        public List<Actor> Actors
        {
            get { return _actors; }
            set { Set(() => Actors, ref _actors, value); }
        }

        private List<TorrentDeserialized> _torrents;

        [DeserializeAs(Name = "torrents")]
        public List<TorrentDeserialized> Torrents
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

        #region Property -> FilePath

        private Uri _filePath;

        /// <summary>
        /// Local path of the downloaded movie file
        /// </summary>
        public Uri FilePath
        {
            get { return _filePath; }
            set { Set(() => FilePath, ref _filePath, value); }
        }

        #endregion

        #region Property -> BackgroundImagePath

        private string _backgroundImagePath = string.Empty;

        /// <summary>
        /// Local path of the downloaded movie's background image
        /// </summary>
        public string BackgroundImagePath
        {
            get { return _backgroundImagePath; }
            set { Set(() => BackgroundImagePath, ref _backgroundImagePath, value); }
        }

        #endregion

        #region Property -> PosterImagePath

        private string _posterImagePath = string.Empty;

        /// <summary>
        /// Local path of the downloaded movie's poster image
        /// </summary>
        public string PosterImagePath
        {
            get { return _posterImagePath; }
            set { Set(() => PosterImagePath, ref _posterImagePath, value); }
        }

        #endregion

        #region Property -> WatchInFullHdQuality


        private bool _watchInFullHdQuality;

        /// <summary>
        /// Decide if movie has to be watched in full HQ or not
        /// </summary>
        public bool WatchInFullHdQuality
        {
            get { return _watchInFullHdQuality; }
            set { Set(() => WatchInFullHdQuality, ref _watchInFullHdQuality, value); }
        }

        #endregion

        #region Property -> FullHdAvailable

        private bool _fullHdAvailable;

        /// <summary>
        /// Indicate if full HQ quality is available
        /// </summary>
        public bool FullHdAvailable
        {
            get { return _fullHdAvailable; }
            set { Set(() => FullHdAvailable, ref _fullHdAvailable, value); }
        }

        #endregion

        #region Property -> AvailableSubtitles

        private ObservableCollection<Subtitle.Subtitle> _availableSubtitles =
            new ObservableCollection<Subtitle.Subtitle>();

        /// <summary>
        /// Available subtitles
        /// </summary>
        public ObservableCollection<Subtitle.Subtitle> AvailableSubtitles
        {
            get { return _availableSubtitles; }
            set { Set(() => AvailableSubtitles, ref _availableSubtitles, value); }
        }

        #endregion

        #region Property -> SelectedSubtitle

        private Subtitle.Subtitle _selectedSubtitle;

        /// <summary>
        /// Selected subtitle
        /// </summary>
        public Subtitle.Subtitle SelectedSubtitle
        {
            get { return _selectedSubtitle; }
            set { Set(() => SelectedSubtitle, ref _selectedSubtitle, value); }
        }

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

        #endregion
    }
}