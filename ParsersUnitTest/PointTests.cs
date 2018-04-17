using Parsers;
using System;
using Xunit;

namespace ParsersUnitTest
{
    public class PointTests
    {
        [Theory]
        [InlineData("A1", 0, 0)]
        [InlineData("b2", 1, 1)]
        [InlineData("C5", 2, 4)]
        [InlineData("AD5", 29, 4)]
        [InlineData("C13", 2, 12)]
        [InlineData("C200", 2, 199)]
        public void Constructor_ValidValue_SetsXAndY(string label, int x, int y)
        {
            var point = new Point(label);

            Assert.Equal(x, point.X);
            Assert.Equal(y, point.Y);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("4")]
        [InlineData("&")]
        [InlineData("AA")]
        [InlineData("44")]
        [InlineData("&&")]
        [InlineData("4A")]
        [InlineData("A0")]
        [InlineData("Ü1")]
        [InlineData("A4A")]
        public void Constructor_InvalidValue_ThrowsException(string label)
        {
            Assert.ThrowsAny<Exception>(() => new Point(label));
        }
    }
}
