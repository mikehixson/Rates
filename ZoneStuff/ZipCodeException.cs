using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneStuff
{
    public struct ZipCodeException : IEquatable<ZipCodeException>
    {
        public ZipCodeRange Origin { get; private set; }
        public ZipCodeRange Destination { get; private set; }
        public byte Zone { get; private set; }
        public byte MailType { get; private set; }

        public ZipCodeException(ZipCodeRange origin, ZipCodeRange destination, byte zone, byte mailType)
        {
            //if (origin == null)
            //    throw new ArgumentNullException(nameof(origin));

            //if (destination == null)
            //    throw new ArgumentNullException(nameof(destination));
            
            Origin = origin;
            Destination = destination;
            Zone = zone;
            MailType = mailType;
        }

        public override string ToString()
        {
            return $"{Origin} {Destination} {Zone} {MailType}";
        }

        public bool Equals(ZipCodeException other)
        {
            return other.Origin.Equals(Origin) && other.Destination.Equals(Destination) && other.Zone == Zone && other.MailType == MailType;
        }
    }

    public class OrderByOriginComparer : IComparer<ZipCodeException>
    {
        public int Compare(ZipCodeException x, ZipCodeException y)
        {
            int compare = 0;

            compare = x.Origin.CompareTo(y.Origin);

            if (compare != 0)
                return compare;

            compare = x.Destination.CompareTo(y.Destination);

            if (compare != 0)
                return compare;

            compare = x.Zone.CompareTo(y.Zone);

            if (compare != 0)
                return compare;

            compare = x.MailType.CompareTo(y.MailType);

            if (compare != 0)
                return compare;

            return 0;
        }
    }

    public class OrderByDestinationComparer : IComparer<ZipCodeException>
    {
        public int Compare(ZipCodeException x, ZipCodeException y)
        {
            int compare = 0;

            compare = x.Destination.CompareTo(y.Destination);

            if (compare != 0)
                return compare;

            compare = x.Origin.CompareTo(y.Origin);

            if (compare != 0)
                return compare;

            compare = x.Zone.CompareTo(y.Zone);

            if (compare != 0)
                return compare;

            compare = x.MailType.CompareTo(y.MailType);

            if (compare != 0)
                return compare;

            return 0;
        }
    }
}
