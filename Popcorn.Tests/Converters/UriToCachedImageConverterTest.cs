using System.Windows.Media.Imaging;
using NUnit.Framework;
using Popcorn.Converters;

namespace Popcorn.Tests.Converters
{
    [TestFixture]
    public class UriToCachedImageConverterTest
    {
        private UriToCachedImageConverter _converter;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _converter = new UriToCachedImageConverter();
        }

        [Test]
        public void Convert_SimpleValue_ReturnsMultipliedValueWithRatio()
        {
            var value = "http://www.google.com/";

            var result = _converter.Convert(value, null, null, null);
            Assert.That(result, Is.TypeOf<BitmapImage>());

            var image = (BitmapImage) result;
            Assert.Equals(image.UriSource.ToString(), value);
        }
    }
}