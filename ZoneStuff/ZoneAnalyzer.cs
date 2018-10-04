using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace ZoneStuff
{
    public class ZoneAnalyzer
    {
        private ZipCodeZone[] _zones;

        public void Run()
        {
            _zones = FileObserver.AllLines(@"C:\Users\Mike\Downloads\charts\Format2.txt", ParseZone)
                .Skip(1)
                .SelectMany(z => z)
                .ToEnumerable()
                .ToArray();

            Console.WriteLine(_zones.Count());

            CreateValidThreeDigitZipFile();
            //MissingOrigin();
            //MissingDestination();
            //MissingOriginAndDestination();
            InverseComparison();
        }

        private void CreateValidThreeDigitZipFile()
        {
            var q = Enumerable.Range(1, 999)
                .Select(i => (Zip: $"{i:000}", Origin: _zones.Count(z => z.Origin == i), Destination: _zones.Count(z => z.Destination == i && z.Number > 0)))
                .Where(v => v.Origin != 0 && v.Destination != 0);

            using (var writer = new StreamWriter(@"C:\Users\Mike\Downloads\charts\valid-zip3.txt"))
            {
                foreach (var value in q)
                    writer.WriteLine(value.Zip);
            }
        }

        private void MissingOrigin()
        {
            var q = Enumerable.Range(1, 999)
                .Select(i => (Zip: $"{ i:000}", Origin: _zones.Count(z => z.Origin == i), Destination: _zones.Count(z => z.Destination == i)))
                .Where(v => v.Origin != 999);

            foreach (var value in q)
                Console.WriteLine($"{value.Zip} {value.Origin} {value.Destination}");

            Console.WriteLine($"Total Missing Origins: {q.Count()}");
        }

        private void MissingDestination()
        {
            var q = Enumerable.Range(1, 999)
            .Select(i => (Zip: i, Sum: _zones.Where(z => z.Destination == i).Sum(z => z.Number)))
            .Where(v => v.Sum == 0);

            foreach (var value in q)
                Console.WriteLine($"{value.Zip} {value.Sum}");

            Console.WriteLine($"Total Missing Origins: {q.Count()}");

        }

        private void MissingOriginAndDestination()
        {
            var q = Enumerable.Range(1, 999)
                .Select(i => (Zip: $"{i:000}", Origin: _zones.Count(z => z.Origin == i), Destination: _zones.Count(z => z.Destination == i && z.Number > 0)))
                .Where(v => v.Origin == 0 || v.Destination == 0);

            foreach (var value in q)
                Console.WriteLine($"{value.Zip} {value.Origin} {value.Destination}");

            Console.WriteLine($"Total Missing Origins: {q.Count()}");
        }

        // See how Origin to Destination compared to Destination to Origin
        private void InverseComparison()
        {
            var invalid = Invalid();

            var q = _zones.Where(z => !invalid.Contains(z.Origin) && !invalid.Contains(z.Destination))
                .Select(a => (A: a, B: _zones.Single(b => b.Origin == a.Destination && b.Destination == a.Origin)))
                .Where(v => v.A.Number != v.B.Number || v.A.Code != v.B.Code)
                .ToArray();

            foreach (var value in q)
                Console.WriteLine($"{value.A} {value.B}");

            Console.WriteLine($"Total Differences: {q.Count()}");
        }

        private int[] Invalid()
        {
            return Enumerable.Range(1, 999)
             .Select(i => (Zip: i, Sum: _zones.Where(z => z.Destination == i).Sum(z => z.Number)))
             .Where(v => v.Sum == 0)
             .Select(v => v.Zip)
             .ToArray();
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





