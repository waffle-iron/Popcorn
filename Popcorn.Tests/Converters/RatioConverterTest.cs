using System.Globalization;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Popcorn.Converters;

namespace Popcorn.Tests.Converters
{
    [TestFixture]
    public class RatioConverterTest
    {
        private RatioConverter _converter;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _converter = new RatioConverter();
        }

        [Test]
        public void Convert_SimpleValue_ReturnsMultipliedValueWithRatio()
        {
            var fixture = new Fixture();
            var value = fixture.Create<double>();
            var parameter = fixture.Create<double>();

            var result = _converter.Convert(value, null, parameter, null);

            Assert.Equals(result, value*parameter);
            Assert.That(result, Is.TypeOf<double>());
        }
    }
}