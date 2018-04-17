using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        private const decimal MilinchPerInch = 1000M;
        private const decimal MilinchPerFoot = 12000M;

        /// <summary>
        /// Gets the distance expressed in milinches.
        /// </summary>
        public int TotalMilinches { get; }

        /// <summary>
        /// Gets the distance expressed in inches.
        /// </summary>
        public decimal TotalInches
        {
            get { return TotalMilinches / MilinchPerInch; }
        }

        /// <summary>
        /// Gets the distance expressed in feet.
        /// </summary>
        public decimal TotalFeet
        {
            get { return TotalMilinches / MilinchPerFoot; }
        }
        
        /// <summary>
        /// Gets the feet component.
        /// </summary>
        public int Feet
        {
            get { return TotalMilinches / (int)MilinchPerFoot; }
        }

        /// <summary>
        /// Gets the inches component.
        /// </summary>
        public int Inches
        {
            get { return (TotalMilinches % (int)MilinchPerFoot) / (int)MilinchPerInch; }
        }

        /// <summary>
        /// Gets the milinches component.
        /// </summary>
        public int Milinches
        {
            get { return (TotalMilinches % (int)MilinchPerInch); }
        }

        public Distance(int milinches)
        {
            TotalMilinches = milinches;
        }

        /// <summary>
        /// Creates a new instance from inches.
        /// </summary>
        /// <param name="inches">distance in inches.</param>
        /// <returns></returns>
        public static Distance FromInches(decimal inches)
        {
            return new Distance((int)(inches * MilinchPerInch));
        }

        /// <summary>
        /// Creates a new instance from feet.
        /// </summary>
        /// <param name="feet">distance in feet.</param>
        /// <returns></returns>
        public static Distance FromFeet(decimal feet)
        {
            return new Distance((int)(feet * MilinchPerFoot));
        }

        /// <summary>
        /// Creates a new instance from feet, inches and milinches.
        /// </summary>
        /// <param name="feet">The foot component of the distance.</param>
        /// <param name="inches">The inches component of the distance.</param>
        /// <param name="milinches">The milinches component of the distance.</param>
        /// <returns></returns>
        public static Distance FromComponents(int feet, int inches, int milinches)
        {
            return new Distance((feet * (int)MilinchPerFoot) + (inches * (int)MilinchPerInch) + milinches);
        }

        public Distance RoundDownToNearest(Distance unit)
        {
            var rounded = (int)Math.Floor(TotalMilinches / (decimal)unit.TotalMilinches) * unit.TotalMilinches;
            return new Distance(rounded);
        }


        public override bool Equals(object obj)
        {
            if (obj is Distance)
                return Equals((Distance)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return TotalMilinches.GetHashCode();
        }

        public bool Equals(Distance other)
        {
            return TotalMilinches == other.TotalMilinches;
        }

        public int CompareTo(Distance other)
        {
            return TotalMilinches.CompareTo(other.TotalMilinches);
        }

        public static bool operator ==(Distance lhs, Distance rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Distance lhs, Distance rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public static bool operator >(Distance lhs, Distance rhs)
        {
            return lhs.TotalMilinches > rhs.TotalMilinches;
        }

        public static bool operator >=(Distance lhs, Distance rhs)
        {
            return lhs.TotalMilinches >= rhs.TotalMilinches;
        }

        public static bool operator <(Distance lhs, Distance rhs)
        {
            return lhs.TotalMilinches < rhs.TotalMilinches;
        }

        public static bool operator <=(Distance lhs, Distance rhs)
        {
            return lhs.TotalMilinches <= rhs.TotalMilinches;
        }

        public static Distance operator +(Distance lhs, Distance rhs)
        {
            return new Distance(lhs.TotalMilinches + rhs.TotalMilinches);
        }

        public static Distance operator -(Distance lhs, Distance rhs)
        {
            return new Distance(lhs.TotalMilinches - rhs.TotalMilinches);
        }

        public static Distance operator *(Distance lhs, int rhs)
        {
            return new Distance(lhs.TotalMilinches * rhs);
        }

        public static Distance operator *(int lhs, Distance rhs)
        {
            return new Distance(lhs * rhs.TotalMilinches);
        }
    }
}
