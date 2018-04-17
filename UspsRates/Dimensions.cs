using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public class Dimensions
    {
        public Distance Length { get; private set; }
        public Distance Width { get; private set; }
        public Distance Height { get; private set; }

        public Distance Girth    // Distance around
        {
            get { return (Width + Height) * 2; }
        }

        public decimal VolumeInCubicInches
        {
            get { return Length.TotalInches * Width.TotalInches * Height.TotalInches; }
        }

        public decimal VolumeInCubicFeet
        {
            get { return Length.TotalFeet * Width.TotalFeet * Height.TotalFeet; }
        }

        // 123.1.4. //TODO: 1.4.2 Determining Dimensional Weight for Nonrectangular Parcels
        public Weight DimensionalWeight
        {
            get
            {
                var volume = Math.Round(Length.TotalInches) * Math.Round(Width.TotalInches) * Math.Round(Height.TotalInches);       //todo: move rounding out of here

                return Weight.FromPounds(volume / 194);
            }
        }

        public Dimensions(Distance a, Distance b, Distance c)
        {
            var values = new[] { a, b, c };

            Array.Sort(values);

            Length = values[2];
            Width = values[1];
            Height = values[0];
        }

        public Dimensions RoundDownToNearest(Distance unit)
        {
            var length = Length.RoundDownToNearest(unit);
            var width = Width.RoundDownToNearest(unit);
            var height = Height.RoundDownToNearest(unit);

            return new Dimensions(length, width, height);
        }

        public static Dimensions FromInches(decimal a, decimal b, decimal c)
        {
            return new Dimensions(Distance.FromInches(a), Distance.FromInches(b), Distance.FromInches(c));
        }

        public static Dimensions FromFeet(decimal a, decimal b, decimal c)
        {
            return new Dimensions(Distance.FromFeet(a), Distance.FromFeet(b), Distance.FromFeet(c));
        }
    }
}
