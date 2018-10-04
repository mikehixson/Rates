using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneStuff
{
    public static class Extensions
    {
        public static IEnumerable<ZipCodeException> CompressByOrigin(this IEnumerable<ZipCodeException> input)
        {
            var sorted = input.OrderBy(e => e, new OrderByOriginComparer());

            var a = sorted.First();

            foreach (var b in sorted.Skip(1))
            {
                if (CanCompressByOrigin(a, b))
                {
                    a = new ZipCodeException(a.Origin, new ZipCodeRangeY(a.Destination.Start, b.Destination.End), a.Zone, a.MailType);
                }
                else
                {
                    yield return a;
                    a = b;
                }
            }

            yield return a;
        }

        private static bool CanCompressByOrigin(ZipCodeException a, ZipCodeException b)
        {
            return a.Origin.Equals(b.Origin) && a.Destination.Proceeds(b.Destination) && a.Zone == b.Zone && a.MailType == b.MailType;
        }

        public static IEnumerable<ZipCodeException> CompressByDestination(this IEnumerable<ZipCodeException> input)
        {
            var sorted = input.OrderBy(e => e, new OrderByDestinationComparer());

            var a = sorted.First();

            foreach (var b in sorted.Skip(1))
            {
                if (CanCompressByDestination(a, b))
                {
                    a = new ZipCodeException(ZipCodeRangeY.Combine(a.Origin, b.Origin), a.Destination, a.Zone, a.MailType);
                }
                else
                {
                    yield return a;
                    a = b;
                }
            }

            yield return a;
        }


        private static bool CanCompressByDestination(ZipCodeException a, ZipCodeException b)
        {
            return a.Destination.Equals(b.Destination) && a.Origin.Proceeds(b.Origin) && a.Zone == b.Zone && a.MailType == b.MailType;
        }





        public static IEnumerable<ZipCodeException> Expand(this IEnumerable<ZipCodeException> input)
        {
            foreach (var exception in input)
            {
                if (IsSingleSegment(exception.Origin) && IsSingleSegment(exception.Destination))
                {
                    yield return exception;
                }
                else
                {
                    foreach (var origin in EnumerateRange(exception.Origin))
                    {
                        foreach (var destination in EnumerateRange(exception.Destination))
                            yield return new ZipCodeException(origin, destination, exception.Zone, exception.MailType);
                    }
                }
            }
        }

        private static bool IsSingleSegment(ZipCodeRangeY range)
        {
            return (range.Start / 100) == (range.End / 100);            
        }

        private static IEnumerable<ZipCodeRangeY> EnumerateRange(ZipCodeRangeY range)
        {
            var start = range.Start;
            while (true)
            {
                if (start > range.End)
                    break;

                var end = Math.Min(range.End, RoundToNextHundred(start) - 1);

                yield return new ZipCodeRangeY(start, end);

                start = end + 1;
            }
        }

        private static int RoundToNextHundred(int value)
        {
            return value + (100 - (value % 100));
        }
    }
}
