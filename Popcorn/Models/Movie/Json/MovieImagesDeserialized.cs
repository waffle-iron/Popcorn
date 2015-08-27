using RestSharp.Deserializers;

namespace Popcorn.Models.Movie.Json
{
    public class MovieImagesDeserialized
    {
        [DeserializeAs(Name = "background_image")]
        public string BackgroundImage { get; set; }

        [DeserializeAs(Name = "small_cover_image")]
        public string SmallCoverImage { get; set; }

        [DeserializeAs(Name = "medium_cover_image")]
        public string MediumCoverImage { get; set; }

        [DeserializeAs(Name = "large_cover_image")]
        public string LargeCoverImage { get; set; }

        [DeserializeAs(Name = "medium_screenshot_image1")]
        public string MediumScreenshotImage1 { get; set; }

        [DeserializeAs(Name = "medium_screenshot_image2")]
        public string MediumScreenshotImage2 { get; set; }

        [DeserializeAs(Name = "medium_screenshot_image3")]
        public string MediumScreenshotImage3 { get; set; }

        [DeserializeAs(Name = "large_screenshot_image1")]
        public string LargeScreenshotImage1 { get; set; }

        [DeserializeAs(Name = "large_screenshot_image2")]
        public string LargeScreenshotImage2 { get; set; }

        [DeserializeAs(Name = "large_screenshot_image3")]
        public string LargeScreenshotImage3 { get; set; }
    }
}
