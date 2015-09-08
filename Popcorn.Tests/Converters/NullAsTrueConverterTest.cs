using NUnit.Framework;
using Popcorn.Converters;

namespace Popcorn.Tests.Converters
{
    [TestFixture]
    public class NullAsTrueConverterTest
    {
        private NullAsTrueConverter _converter;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _converter = new NullAsTrueConverter();
        }

        [Test]
        public void Convert_Null_ReturnsTrue()
        {
            Assert.Equals(
                _converter.Convert(null, null, null, null), true);
        }

        [Test]
        public void Convert_NotNull_ReturnsFalse()
        {
            Assert.Equals(
                _converter.Convert(new object(), null, null, null), false);
        }
    }
}