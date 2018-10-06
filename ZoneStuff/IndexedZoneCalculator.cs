using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ZoneStuff
{
    public class IndexedZoneCalculator
    {
        private (ZipCodeZone Zone, ZipCodeException[] Exceptions)[] _zones = new (ZipCodeZone Zone, ZipCodeException[] Exceptions)[1000 * 1000];

        public IndexedZoneCalculator()
        {
            Init();
        }

        public (byte Base, byte Ex) Lookup(int origin, int destination)
        {
            var zone = _zones[GetIndex(origin / 100, destination / 100)];

            return (zone.Zone.Number, zone.Exceptions.SingleOrDefault(e => e.Origin.Contains(origin) && e.Destination.Contains(destination)).Zone);                
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

            var zones = FileObserver.AllLines(@"C:\Users\Mike\Downloads\charts\Format2.txt", ParseZone)
                .Skip(1)
                .SelectMany(z => z)
                .ToEnumerable()
                .Select(z => (Zone: z, Exceptions: exceptions[$"{z.Origin:000}{z.Destination:000}"].ToArray()));

            foreach (var zone in zones)
                _zones[GetIndex(zone.Zone.Origin, zone.Zone.Destination)] = zone;

            Console.WriteLine(_zones.Count());
        }

        private int GetIndex(int origin, int destination)
        {
           return (origin * 1000) + destination;
        }
        

        static int lineIndex = 0;

        private static ZipCodeException ParseException(ReadOnlySequence<byte> line)
        {
            if (lineIndex > 0)
            {
                var originStart = Converter.GetInt32(line.Slice(0, 5));
                var originEnd = Converter.GetInt32(line.Slice(5, 5));
                var destinationStart = Converter.GetInt32(line.Slice(10, 5));
                var destinationEnd = Converter.GetInt32(line.Slice(15, 5));
                var zone = (byte)Converter.GetInt32(line.Slice(20, 2));
                var type = (byte)Converter.GetInt32(line.Slice(22, 2));

                return new ZipCodeException(new ZipCodeRange(originStart, originEnd), new ZipCodeRange(destinationStart, destinationEnd), zone, type);
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
                var origin = (short)Converter.GetInt32(line.Slice(0, 3));

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
