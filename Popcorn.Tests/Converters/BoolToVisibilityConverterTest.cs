﻿using System.Windows;
using NUnit.Framework;
using Popcorn.Converters;

namespace Popcorn.Tests.Converters
{
    [TestFixture]
    public class BoolToVisibilityConverterTest
    {
        private BoolToVisibilityConverter _converter;

        [TestFixtureSetUp]
        public void InitializeConverter()
        {
            _converter = new BoolToVisibilityConverter();
        }

        [Test]
        public void Convert_False_ReturnsVisible()
        {
            Assert.Equals(
                _converter.Convert(false, typeof (Visibility), null, null), Visibility.Visible);
        }

        [Test]
        public void Convert_True_ReturnsCollapsed()
        {
            Assert.Equals(
                _converter.Convert(true, typeof(Visibility), null, null), Visibility.Collapsed);
        }
    }
}