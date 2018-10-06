using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneStuff
{
    public struct ZipCodeRange : IEquatable<ZipCodeRange>, IComparable<ZipCodeRange>
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public ZipCodeRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Proceeds(ZipCodeRange other)
        {
            return End + 1 == other.Start;
        }

        public bool Contains(int value)
        {
            return value >= Start && value <= End;
        }

        public bool Intersects(ZipCodeRange other)
        {
            return Contains(other.Start) || Contains(other.End) || other.Contains(Start) || other.Contains(End);
        }

        public bool Equals(ZipCodeRange other)
        {
            //if (other == null)
            //    return false;

            return other.Start == Start && other.End == End;
        }

        public int CompareTo(ZipCodeRange other)
        {
            int compare;

            compare = Start.CompareTo(other.Start);

            if (compare != 0)
                return compare;

            compare = End.CompareTo(other.End);

            if (compare != 0)
                return compare;

            return 0;
        }

        public override bool Equals(object obj)
        {
            //return Equals(obj as ZipCodeRangeY);
            return Equals((ZipCodeRange)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                
                hash = (hash * 16777619) ^ Start.GetHashCode();
                hash = (hash * 16777619) ^ End.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return $"{Start:00000} - {End:00000}";
        }

        public static ZipCodeRange Combine(ZipCodeRange a, ZipCodeRange b)
        {
            return new ZipCodeRange(a.Start, b.End);
        }
    }
}
