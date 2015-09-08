using NUnit.Framework;
using Ploeh.AutoFixture;
using Popcorn.Comparers;
using Popcorn.Models.Movie.Short;

namespace Popcorn.Tests.Comparers
{
    [TestFixture]
    public class MovieShortComparerTest
    {
        private MovieShortComparer _comparer;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _comparer = new MovieShortComparer();
        }

        [Test]
        public void Equals_SameMovie_ReturnsTrue()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<MovieShort>();
            Assert.Equals(
                _comparer.Equals(movie, movie), true);
        }

        [Test]
        public void Equals_DifferentMovies_ReturnsFalse()
        {
            var fixture = new Fixture();
            var movie1 = fixture.Create<MovieShort>();
            var movie2 = fixture.Create<MovieShort>();
            Assert.Equals(
                _comparer.Equals(movie1, movie2), false);
        }

        [Test]
        public void GetHashCode_SameMovie_ReturnsSameHashCode()
        {
            var fixture = new Fixture();
            var movie = fixture.Create<MovieShort>();
            Assert.Equals(
                _comparer.GetHashCode(movie), _comparer.GetHashCode(movie));
        }

        [Test]
        public void GetHashCode_DifferentMovies_ReturnsDifferentHashCode()
        {
            var fixture = new Fixture();
            var movie1 = fixture.Create<MovieShort>();
            var movie2 = fixture.Create<MovieShort>();
            Assert.AreNotEqual(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie2));
        }
    }
}
