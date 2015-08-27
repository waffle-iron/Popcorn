using GalaSoft.MvvmLight;
using RestSharp.Deserializers;

namespace Popcorn.Models.Images
{
    public class MovieImages : ObservableObject
    {
        private string _backgroundImage;

        [DeserializeAs(Name = "background_image")]
        public string BackgroundImage
        {
            get { return _backgroundImage; }
            set { Set(() => BackgroundImage, ref _backgroundImage, value); }
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

        private string _largeCoverImage;

        [DeserializeAs(Name = "large_cover_image")]
        public string LargeCoverImage
        {
            get { return _largeCoverImage; }
            set { Set(() => LargeCoverImage, ref _largeCoverImage, value); }
        }

        private string _mediumScreenshotImage1;

        [DeserializeAs(Name = "medium_screenshot_image1")]
        public string MediumScreenshotImage1
        {
            get { return _mediumScreenshotImage1; }
            set { Set(() => MediumScreenshotImage1, ref _mediumScreenshotImage1, value); }
        }


        private string _mediumScreenshotImage2;

        [DeserializeAs(Name = "medium_screenshot_image2")]
        public string MediumScreenshotImage2
        {
            get { return _mediumScreenshotImage2; }
            set { Set(() => MediumScreenshotImage2, ref _mediumScreenshotImage2, value); }
        }

        private string _mediumScreenshotImage3;

        [DeserializeAs(Name = "medium_screenshot_image3")]
        public string MediumScreenshotImage3
        {
            get { return _mediumScreenshotImage3; }
            set { Set(() => MediumScreenshotImage3, ref _mediumScreenshotImage3, value); }
        }

        private string _largeScreenshotImage1;

        [DeserializeAs(Name = "large_screenshot_image1")]
        public string LargeScreenshotImage1
        {
            get { return _largeScreenshotImage1; }
            set { Set(() => LargeScreenshotImage1, ref _largeScreenshotImage1, value); }
        }

        private string _largeScreenshotImage2;

        [DeserializeAs(Name = "large_screenshot_image2")]
        public string LargeScreenshotImage2
        {
            get { return _largeScreenshotImage2; }
            set { Set(() => LargeScreenshotImage2, ref _largeScreenshotImage2, value); }
        }

        private string _largeScreenshotImage3;

        [DeserializeAs(Name = "large_screenshot_image3")]
        public string LargeScreenshotImage3
        {
            get { return _largeScreenshotImage3; }
            set { Set(() => LargeScreenshotImage3, ref _largeScreenshotImage3, value); }
        }
    }
}
