using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Popcorn.Entity.Movie
{
    /// <summary>
    /// Represents a movie with all its details
    /// </summary>
    public class MovieFull
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int MovieId { get; set; }
        public bool IsFavorite { get; set; }
        public bool HasBeenSeen { get; set; }
        public string Url { get; set; }
        public string ImdbCode { get; set; }
        public string Title { get; set; }
        public string TitleLong { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public int Runtime { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public string Language { get; set; }
        public string MpaRating { get; set; }
        public string DownloadCount { get; set; }
        public string LikeCount { get; set; }
        public string RtCrtiticsScore { get; set; }
        public string RtCriticsRating { get; set; }
        public string RtAudienceScore { get; set; }
        public string RtAudienceRating { get; set; }
        public string DescriptionIntro { get; set; }
        public string DescriptionFull { get; set; }
        public string YtTrailerCode { get; set; }
        public string BackgroundImage { get; set; }
        public string SmallCoverImage { get; set; }
        public string MediumCoverImage { get; set; }
        public string LargeCoverImage { get; set; }
        public string MediumScreenshotImage1 { get; set; }
        public string MediumScreenshotImage2 { get; set; }
        public string MediumScreenshotImage3 { get; set; }
        public string LargeScreenshotImage1 { get; set; }
        public string LargeScreenshotImage2 { get; set; }
        public string LargeScreenshotImage3 { get; set; }
        public virtual ICollection<Cast.Cast> Cast { get; set; }
        public virtual ICollection<Torrent> Torrents { get; set; }
        public string DateUploaded { get; set; }
        public int DateUploadedUnix { get; set; }
    }
}