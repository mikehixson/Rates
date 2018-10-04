using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ZoneStuff
{
    public class BruteForceZoneCalculator
    {
        private IEnumerable<(ZipCodeZone Zone, ZipCodeException[] Exceptions)> _zones;

        public BruteForceZoneCalculator()
        {
            Init();
        }

        public (byte Base, byte Ex) Lookup(int origin, int destination)
        {
            var zone = _zones.Single(f => f.Zone.Origin == origin / 100 && f.Zone.Destination == destination / 100);

            return (zone.Zone.Number, zone.Exceptions.SingleOrDefault(e => e.Origin.Contains(origin) && e.Destination.Contains(destination)).Zone);                
        }

        public ZipCodeException[] ListExceptions(int origin, int destination)
        {
            var exceptions = _zones.Single(r => r.Zone.Origin == origin && r.Zone.Destination == destination).Exceptions;

            foreach(var exception in exceptions)
                Console.WriteLine(exception);

            return exceptions;
        }

        private void Init()
        {
            var exceptions = FileObserver.AllLines(@"C:\Users\Mike\Downloads\charts\exception.txt", ParseException)
                .Skip(1)
                .ToEnumerable()
                .ToArray()
                .CompressByOrigin()
                .CompressByDestination()
                .Expand()
                .ToLookup(e => $"{e.Origin.Start / 100:000}{e.Destination.Start / 100:000}");

            Console.WriteLine(exceptions.Count);
            Console.WriteLine(exceptions.Sum(e => e.Count()));



            _zones = FileObserver.AllLines(@"C:\Users\Mike\Downloads\charts\Format2.txt", ParseZone)
                .Skip(1)
                .SelectMany(z => z)
                .ToEnumerable()
                .Select(z =>  (z, exceptions[$"{z.Origin:000}{z.Destination:000}"].ToArray()))
                .ToArray();

            Console.WriteLine(_zones.Count());
        }

        

        static int lineIndex = 0;

        private static ZipCodeException ParseException(ReadOnlySequence<byte> line)
        {
            if (lineIndex > 0)
            {
                var originStart = MyReader.GetInt32(line.Slice(0, 5));
                var originEnd = MyReader.GetInt32(line.Slice(5, 5));
                var destinationStart = MyReader.GetInt32(line.Slice(10, 5));
                var destinationEnd = MyReader.GetInt32(line.Slice(15, 5));
                var zone = (byte)MyReader.GetInt32(line.Slice(20, 2));
                var type = (byte)MyReader.GetInt32(line.Slice(22, 2));

                return new ZipCodeException(new ZipCodeRangeY(originStart, originEnd), new ZipCodeRangeY(destinationStart, destinationEnd), zone, type);
            }
            else
            {
                lineIndex++;
                return new ZipCodeException();
            }
        }

        static int lineIndex2 = 0;

        private static List<ZipCodeZone> ParseZone(ReadOnlySequence<byte> line)
        {
            var zones = new List<ZipCodeZone>();

            if (lineIndex2 > 0)
            {
                var origin = (short)MyReader.GetInt32(line.Slice(0, 3));

                var destination = (short)1;

                while (true)
                {
                    var index = 3 + ((destination - 1) * 2);

                    if (!(index < line.Length))
                        break;

                    // todo: is there a better way?
                    var number = line.Slice(index, 1).First.Span[0] - 0x30;
                    var code = line.Slice(index + 1, 1).First.Span[0];

                    zones.Add(new ZipCodeZone(origin, destination, (byte)number, code));
                    destination++;
                }
                lineIndex2++;
                return zones;
            }
            else
            {
                lineIndex2++;
                return null;
            }
        }
    }
}
