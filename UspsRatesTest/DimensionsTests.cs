using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using Xunit;

namespace UspsRatesTest
{
    public class DimensionsTests
    {
        [Fact]
        public void Length_WhenCalled_IsLargest()
        {
            var dim = new Dimensions(Distance.FromInches(5), Distance.FromInches(10), Distance.FromInches(3));

            Assert.Equal(Distance.FromInches(10), dim.Length);
        }

        [Fact]
        public void Width_WhenCalled_IsSecondLargest()
        {
            var dim = new Dimensions(Distance.FromInches(5), Distance.FromInches(10), Distance.FromInches(3));

            Assert.Equal(Distance.FromInches(5), dim.Width);
        }

        [Fact]
        public void Height_WhenCalled_IsThirdLargest()
        {
            var dim = new Dimensions(Distance.FromInches(5), Distance.FromInches(10), Distance.FromInches(3));

            Assert.Equal(Distance.FromInches(3), dim.Height);
        }

        [Fact]
        public void Volume_WhenCalled_IsAccruate()
        {
            var dim = new Dimensions(Distance.FromInches(5), Distance.FromInches(10), Distance.FromInches(3));

            Assert.Equal(5 * 10 * 3, dim.VolumeInCubicInches);
        }

        [Fact]
        public void Girth_WhenCalled_IsAccruate()
        {
            var dim = new Dimensions(Distance.FromInches(5), Distance.FromInches(10), Distance.FromInches(3));

            Assert.Equal(Distance.FromInches((5 + 3) * 2), dim.Girth);
        }

        [Fact]
        public void RoundDownToNearest_WhenCalled_IsAccruate()
        {
            var dim = Dimensions.FromInches(3.9M, 2.3M, 1.1M).RoundDownToNearest(Distance.FromInches(0.25M));

            Assert.Equal(3.75M, dim.Length.TotalInches);
            Assert.Equal(2.25M, dim.Width.TotalInches);
            Assert.Equal(1M, dim.Height.TotalInches);

        }
    }
}
