using NUnit.Framework;
using Popcorn.Converters;

namespace Popcorn.Tests.Converters
{
    [TestFixture]
    public class MultiBooleanToBoolConverterTest
    {
        private MultiBooleanToBoolConverter _converter;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _converter = new MultiBooleanToBoolConverter();
        }

        [Test]
        public void Convert_WithAllTrue_ReturnsTrue()
        {
            object[] trueBooleans = { true, true, true };
            Assert.Equals(
                _converter.Convert(trueBooleans, null, null, null), true);
        }

        [Test]
        public void Convert_WithAllFalse_ReturnsFalse()
        {
            object[] falseBooleans = { false, false, false };
            Assert.Equals(
                _converter.Convert(falseBooleans, null, null, null), false);
        }

        [Test]
        public void Convert_WithNotAllTrue_ReturnsFalse()
        {
            object[] notAllTrueBooleans = { true, false, true };
            Assert.Equals(
                _converter.Convert(notAllTrueBooleans, null, null, null), false);
        }
    }
}
