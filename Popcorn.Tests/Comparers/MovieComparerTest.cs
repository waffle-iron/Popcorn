using NUnit.Framework;
using Ploeh.AutoFixture;
using Popcorn.Comparers;
using Popcorn.Models.Movie;

namespace Popcorn.Tests.Comparers
{
    [TestFixture]
    public class MovieComparerTest
    {
        private MovieComparer _comparer;

        [OneTimeSetUp]
        public void InitializeConverter()
        {
            _comparer = new MovieComparer();
        }

        [Test]
        public void Equals_SameMovie_ReturnsTrue()
        {
            var fixture = new Fixture();

            var movie1 = new MovieJson();
            var movie2 = new MovieJson();

            var imdbCode = fixture.Create<string>();
            var dateUploadedUnix = fixture.Create<int>();

            movie1.ImdbCode = imdbCode;
            movie2.ImdbCode = imdbCode;
            movie1.DateUploadedUnix = dateUploadedUnix;
            movie2.DateUploadedUnix = dateUploadedUnix;

            Assert.AreEqual(
                _comparer.Equals(movie1, movie1), true);

            Assert.AreEqual(
                _comparer.Equals(movie1, movie2), true);
        }

        [Test]
        public void Equals_DifferentMovies_ReturnsFalse()
        {
            var fixture = new Fixture();

            var id1 = fixture.Create<string>();
            var dateUploadedUnix1 = fixture.Create<int>();

            var id2 = fixture.Create<string>();
            var dateUploadedUnix2 = fixture.Create<int>();

            var movie1 = new MovieJson
            {
                ImdbCode = id1,
                DateUploadedUnix = dateUploadedUnix1
            };

            var movie2 = new MovieJson
            {
                ImdbCode = id2,
                DateUploadedUnix = dateUploadedUnix2
            };

            Assert.AreEqual(
                _comparer.Equals(movie1, movie2), false);

            Assert.AreEqual(
                _comparer.Equals(movie1, null), false);

            Assert.AreEqual(
                _comparer.Equals(movie2, null), false);
        }

        [Test]
        public void GetHashCode_SameMovie_ReturnsSameHashCode()
        {
            var fixture = new Fixture();

            var movie1 = new MovieJson();
            var movie2 = new MovieJson();

            var id = fixture.Create<string>();
            var dateUploadedUnix = fixture.Create<int>();

            movie1.ImdbCode = id;
            movie2.ImdbCode = id;
            movie1.DateUploadedUnix = dateUploadedUnix;
            movie2.DateUploadedUnix = dateUploadedUnix;

            Assert.AreEqual(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie1));

            Assert.AreEqual(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie2));
        }

        [Test]
        public void GetHashCode_DifferentMovies_ReturnsDifferentHashCode()
        {
            var fixture = new Fixture();

            var id1 = fixture.Create<string>();
            var dateUploadedUnix1 = fixture.Create<int>();

            var id2 = fixture.Create<string>();
            var dateUploadedUnix2 = fixture.Create<int>();

            var movie1 = new MovieJson
            {
                ImdbCode = id1,
                DateUploadedUnix = dateUploadedUnix1
            };

            var movie2 = new MovieJson
            {
                ImdbCode = id2,
                DateUploadedUnix = dateUploadedUnix2
            };

            Assert.AreNotEqual(
                _comparer.GetHashCode(movie1), _comparer.GetHashCode(movie2));
        }
    }
}
