using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public struct Weight : IEquatable<Weight>, IComparable<Weight>
    {
        private const decimal MilouncePerOnce = 1000M;
        private const decimal MilouncePerPound = 16000M;

        /// <summary>
        /// Gets an instance rpresenting zero weight.
        /// </summary>
        public static Weight Zero = new Weight(0);

        /// <summary>
        /// Gets the weight expressed in milounces.
        /// </summary>
        public int TotalMilounces { get; }

        /// <summary>
        /// Gets the weight expressed in ounces.
        /// </summary>
        public decimal TotalOunces
        {
            get { return TotalMilounces / MilouncePerOnce; }
        }

        /// <summary>
        /// Gets the weight expressed in pinds.
        /// </summary>
        public decimal TotalPounds
        {
            get { return TotalMilounces / MilouncePerPound; }
        }

        /// <summary>
        /// Gets the pounds component.
        /// </summary>
        public int Pounds
        {
            get { return TotalMilounces / (int)MilouncePerPound; }
        }

        /// <summary>
        /// Gets the ounces component.
        /// </summary>
        public int Ounces
        {
            get { return (TotalMilounces % (int)MilouncePerPound) / (int)MilouncePerOnce; }
        }

        /// <summary>
        /// Gets the milounces component.
        /// </summary>
        public int Milounces
        {
            get { return (TotalMilounces % (int)MilouncePerOnce); }
        }

        private Weight(int milounces)
        {
            if (milounces < 0)
                throw new ArgumentOutOfRangeException("Weight must be a positive value.");

            TotalMilounces = milounces;
        }

        /// <summary>
        /// Creates a new instance from ounces.
        /// </summary>
        /// <param name="ounces">The weight in ounces.</param>
        /// <returns></returns>
        public static Weight FromOunces(decimal ounces)
        {
            return new Weight((int)(ounces * MilouncePerOnce));
        }

        /// <summary>
        /// Creates a new instance from pounds.
        /// </summary>
        /// <param name="pounds">The weight in ounces.</param>
        /// <returns></returns>
        public static Weight FromPounds(decimal pounds)
        {
            return new Weight((int)(pounds * MilouncePerPound));
        }

        /// <summary>
        /// Creates a new instance from pounds, ounces and milounces.
        /// </summary>
        /// <param name="pounds">The pounds component of the weight.</param>
        /// <param name="ounces">The ounces component of the weight.</param>
        /// <param name="milounces">The milounces component of the weight.</param>
        /// <returns></returns>
        public static Weight FromComponents(int pounds, int ounces, int milounces)
        {
            return new Weight((pounds * (int)MilouncePerPound) + (ounces * (int)MilouncePerOnce) + milounces);
        }

        /// <summary>
        /// Returns the larger of two weights.
        /// </summary>
        /// <param name="a">The first weight to compare.</param>
        /// <param name="b">The second weight to compare.</param>
        /// <returns></returns>
        public static Weight Max(Weight a, Weight b)
        {
            if (a > b)
                return a;

            return b;
        }

        public override bool Equals(object obj)
        {
            if (obj is Weight)
                return Equals((Weight)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return TotalMilounces.GetHashCode();
        }

        public bool Equals(Weight other)
        {
            return TotalMilounces == other.TotalMilounces;
        }

        public int CompareTo(Weight other)
        {
            return TotalMilounces.CompareTo(other.TotalMilounces);
        }

        public static bool operator ==(Weight lhs, Weight rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Weight lhs, Weight rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public static bool operator >(Weight lhs, Weight rhs)
        {
            return lhs.TotalMilounces > rhs.TotalMilounces;
        }

        public static bool operator >=(Weight lhs, Weight rhs)
        {
            return lhs.TotalMilounces >= rhs.TotalMilounces;
        }

        public static bool operator <(Weight lhs, Weight rhs)
        {
            return lhs.TotalMilounces < rhs.TotalMilounces;
        }

        public static bool operator <=(Weight lhs, Weight rhs)
        {
            return lhs.TotalMilounces <= rhs.TotalMilounces;
        }
    }
}
