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

            var movie1 = fixture.Create<MovieShort>();
            var movie2 = fixture.Create<MovieShort>();

            var id = fixture.Create<int>();
            var dateUploadedUnix = fixture.Create<int>();

            movie1.Id = id;
            movie2.Id = id;
            movie1.DateUploadedUnix = dateUploadedUnix;
            movie2.DateUploadedUnix = dateUploadedUnix;

            Assert.Equals(
                _comparer.Equals(movie1, movie1), true);

            Assert.Equals(
                _comparer.Equals(movie1, movie2), true);
        }

        [Test]
        public void Equals_DifferentMovies_ReturnsFalse()
        {
            var fixture = new Fixture();

            var movie1 = fixture.Create<MovieShort>();
            var movie2 = fixture.Create<MovieShort>();

            Assert.Equals(
                _comparer.Equals(movie1, movie2), false);

            Assert.Equals(
                _comparer.Equals(movie1, null), false);

            Assert.Equals(
                _comparer.Equals(movie2, null), false);
        }

        [Test]
        public void GetHashCode_SameMovie_ReturnsSameHashCode()
        {
            var fixture = new Fixture();

            var movie1 = fixture.Create<MovieShort>();
            var movie2 = fixture.Create<MovieShort>();

            var id = fixture.Create<int>();
            var dateUploadedUnix = fixture.Create<int>();

            movie1.Id = id;
            movie2.Id = id;
            movie1.DateUploadedUnix = dateUploadedUnix;
            movie2.DateUploadedUnix = dateUploadedUnix;

            Assert.Equals(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie1));

            Assert.Equals(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie2));
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
