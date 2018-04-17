using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using Xunit;

namespace UspsRatesTest
{
    public class DistanceTests
    {
        [InlineData(54, 54000)]
        [InlineData(54.75, 54750)]
        [Theory]        
        public void FromInch_SetsCorrectMilinches(decimal inches, int milinches)
        {
            var distance = Distance.FromInches(inches);

            Assert.Equal(milinches, distance.TotalMilinches);
        }

        [InlineData(7, 84000)]
        [InlineData(7.33, 87960)]
        [Theory]
        public void FromFeet_SetsCorrectMilinches(decimal feet, int milinches)
        {
            var distance = Distance.FromFeet(feet);

            Assert.Equal(milinches, distance.TotalMilinches);
        }

        [InlineData(7, 1, 2, 85002)]
        [InlineData(0, 2, 3, 2003)]
        [InlineData(0, 0, 323, 323)]
        [InlineData(0, 4, 0, 4000)]
        [InlineData(1, 4, 0, 16000)]
        [Theory]
        public void FromComponents_SetsCorrectMilinches(int feet, int inches, int milinches, int totalMilinches)
        {
            var distance = Distance.FromComponents(feet, inches, milinches);

            Assert.Equal(totalMilinches, distance.TotalMilinches);
        }

        [InlineData(54)]
        [InlineData(54.75)]
        [Theory]
        public void TotalInches_GetsCorrectInches(decimal inches)
        {
            var distance = Distance.FromInches(inches);

            Assert.Equal(inches, distance.TotalInches);
        }

        [InlineData(7)]
        [InlineData(7.75)]
        [Theory]
        public void TotalFeet_GetsCorrectFeet(decimal feet)
        {
            var distance = Distance.FromFeet(feet);

            Assert.Equal(feet, distance.TotalFeet);
        }

        [InlineData(7.5, 7, 6, 0)]
        [InlineData(7.812, 7, 9, 744)]
        [InlineData(0.456, 0, 5, 472)]
        [Theory]
        public void ComponentValues_GetCorrectValues(decimal totalFeet, int feet, int inches, int milinches)
        {
            var distance = Distance.FromFeet(totalFeet);

            Assert.Equal(feet, distance.Feet);
            Assert.Equal(inches, distance.Inches);
            Assert.Equal(milinches, distance.Milinches);
        }

        [Fact]
        public void Equals_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromInches(12);

            Assert.True(distanceA.Equals(distanceB));
        }

        [Fact]
        public void CompareTo_WhenIsLessThan_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromFeet(2);

            var compare = distanceA.CompareTo(distanceB);

            Assert.True(compare < 0);
        }

        [Fact]
        public void CompareTo_WhenIsEqualTo_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromFeet(1);

            var compare = distanceA.CompareTo(distanceB);

            Assert.True(compare == 0);
        }

        [Fact]
        public void CompareTo_WhenIsGreaterThan_IsCorrect()
        {
            var distanceA = Distance.FromFeet(2);
            var distanceB = Distance.FromFeet(1);

            var compare = distanceA.CompareTo(distanceB);

            Assert.True(compare > 0);
        }

        [Fact]
        public void GetHashCode_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromInches(12);

            Assert.True(distanceA.GetHashCode() == distanceB.GetHashCode());
        }

        [Fact]
        public void EqualsOperator_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromInches(12);

            Assert.True(distanceA == distanceB);
        }

        [Fact]
        public void NotEqualsOperator_IsCorrect()
        {
            var distanceA = Distance.FromFeet(1);
            var distanceB = Distance.FromInches(12);

            Assert.False(distanceA != distanceB);
        }

        //todo: tests for the rest of the operators.
    }
}
