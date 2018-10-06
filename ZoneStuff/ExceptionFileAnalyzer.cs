using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Threading;

namespace ZoneStuff
{
    public class ExceptionFileAnalyzer
    {
        //SortedSet<ZipCodeException> _list = new SortedSet<ZipCodeException>(new OrderByOriginComparer());
        List<ZipCodeException> _list = new List<ZipCodeException>();

        public async Task Foo()
        {
            //var filePath = @"C:\Users\Mike\Downloads\charts\exception-compressed.txt";  //16440
            //var filePath = @"C:\Users\Mike\Downloads\charts\exception.txt"; //32219
            var filePath = @"C:\Users\Mike\Downloads\charts\exception-expanded.txt";    //101120
            //var filePath = @"C:\Users\Mike\Downloads\charts\exception-expanded3.txt";

            await FileReader.ReadAllLinesAsync(filePath, Process);

            Console.WriteLine($"Starting with: {_list.Count}");

            //DistinctFiveDigitRanges();
            //DistinctTwoDigitRanges();
            //DistinctTwoDigitRangePairs();
            //DistinctZips();
            //DistinctValuesUsingFiveDigitRanges();
            //DistinctValuesUsingTwoDigitRanges();
            //OriginDestinationPairsThatHaveExceptions();
            //ListExceptions(097, 865);
            //TwoDigitLookup();
            //RangesThatOverlap();
            RangesThatDoNotSpanTheFullSegment();
        }

        private void DistinctFiveDigitRanges()
        {
            var query = _list.SelectMany(e => new[] { e.Origin, e.Destination })
                .GroupBy(r => $"{r.Start:00000}{r.End:00000}")
                .OrderBy(g => g.Count());            

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }

        // Requires exception-expanded.txt
        private void DistinctTwoDigitRanges()
        {
            var query = _list.SelectMany(e => new[] { new ZipCodeRange(e.Origin.Start % 100, e.Origin.End % 100), new ZipCodeRange(e.Destination.Start % 100, e.Destination.End % 100) })
                .GroupBy(r => $"{r.Start:00000}{r.End:00000}")
                .OrderBy(g => g.Count());
                       

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }



        private void DistinctTwoDigitRangePairs()
        {
            var query = _list.Select(e => new { A = new ZipCodeRange(e.Origin.Start % 100, e.Origin.End % 100), B = new ZipCodeRange(e.Destination.Start % 100, e.Destination.End % 100) })
                .GroupBy(r => $"{r.A.Start:00000}{r.A.End:00000}{r.B.Start:00000}{r.B.End:00000}")
                .OrderBy(g => g.Count());

            Console.WriteLine($"Distinct Values: {query.Count()}");

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");
        }

        private void DistinctZips()
        {
            var query = _list.SelectMany(e => new[] { e.Origin.Start, e.Origin.End, e.Destination.Start, e.Destination.End })
                .GroupBy(r => r)
                .OrderBy(g => g.Count());

            Console.WriteLine($"Distinct Values: {query.Count()}");

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");
        }

        private void DistinctValuesUsingFiveDigitRanges()
        {
            var query = _list
                .GroupBy(r => $"{r.Origin.Start:00000}{r.Origin.End:00000}{r.Destination.Start:00000}{r.Destination.End:00000}{r.Zone}{r.MailType}")
                .OrderBy(g => g.Count());

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }

        private void DistinctValuesUsingTwoDigitRanges()
        {
            var query = _list
                .GroupBy(r => $"{r.Origin.Start % 100:00}{r.Origin.End % 100:00}{r.Destination.Start % 100:00}{r.Destination.End % 100:00}{r.Zone}{r.MailType}")
                .OrderBy(g => g.Count());

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }
                
        public IEnumerable<IGrouping<string, ZipCodeException>> OriginDestinationPairsThatHaveExceptions()
        {
            var query = _list.SelectMany(e => Expand(e).Distinct())
                .GroupBy(r => $"{r.Origin.Start / 100:000}{r.Destination.Start / 100:000}")
                .OrderBy(g => g.Count());

            foreach (var group in query)
                Console.WriteLine($"{group.Key} {group.Count()}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
            Console.WriteLine($"Sum of Values: {query.Sum(g => g.Count())}");

            return query.ToArray();
        }


        private void ListExceptions(int origin, int destination)
        {
            var query = _list.Where(e => (e.Origin.Start / 100) <= origin && (e.Origin.End / 100) >= origin && (e.Destination.Start / 100) <= destination && (e.Destination.End / 100) >= destination);

            foreach (var group in query)
                Console.WriteLine($"{group.Origin} {group.Destination} {group.Zone} {group.MailType}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }

        // Requires exception-expanded.txt
        private void TwoDigitLookup()
        {
            // Distinct Lookup
            var lookup = _list.Select(r => $"{r.Origin.Start % 100:00}{r.Origin.End % 100:00}{r.Destination.Start % 100:00}{r.Destination.End % 100:00}{r.Zone}{r.MailType}")
                .Distinct()                
                .ToArray();

            var query = _list.SelectMany(e => Expand(e).Distinct())
                .GroupBy(r => $"{r.Origin.Start / 100:000}{r.Destination.Start / 100:000}");

            foreach (var group in query)
            {
                Console.WriteLine(group.Key);

                var k = group.Select(r => $"{r.Origin.Start % 100:00}{r.Origin.End % 100:00}{r.Destination.Start % 100:00}{r.Destination.End % 100:00}{r.Zone}{r.MailType}")
                    .Select(v => Array.IndexOf(lookup, v).ToString())
                    .ToArray();

                Console.WriteLine(String.Join(",", k));


                Console.WriteLine();
            }

            //for(int y = 0; y < lookup.Length / 20; y++)
            //{
            //    for(int x = 0; x < 20; x++)
            //        Console.Write(lookup[(y * 20) + x]);
            //}
        }

        private void RangesThatOverlap()
        {
            int count = 0;
            Parallel.For(0, _list.Count, (i) =>
            {
                var a = _list[i];

                for (var n = i + 1; n < _list.Count(); n++)
                {
                    var b = _list[n];

                    if (a.Origin.Intersects(b.Origin) && a.Destination.Intersects(b.Destination))
                    {
                        Interlocked.Increment(ref count);
                        Console.WriteLine($"{a} => {b}");
                    }
                }
            });

            Console.WriteLine($"Total: {count}");
        }

        public void RangesThatDoNotSpanTheFullSegment()
        {
            var query = _list.Where(e => !(e.Origin.Start % 100 == 0 && e.Origin.End % 100 == 99 || e.Destination.Start % 100 == 0 && e.Destination.End % 100 == 99));

            foreach (var item in query)
                Console.WriteLine($"{item}");

            Console.WriteLine($"Distinct Values: {query.Count()}");
        }




        private IEnumerable<ZipCodeException> Expand(ZipCodeException exception)
        {
            // Origin
            var a = exception.Origin.Start / 100;
            var b = exception.Origin.End / 100;

            // Destination
            var c = exception.Destination.Start / 100;
            var d = exception.Destination.End / 100;

            if (a == b && c == d)
                yield return exception;

            foreach (var origin in EnumerateRange(exception.Origin))
            {
                foreach (var destination in EnumerateRange(exception.Destination))
                    yield return new ZipCodeException(origin, destination, exception.Zone, exception.MailType);
            }
        }

        private IEnumerable<ZipCodeRange> EnumerateRange(ZipCodeRange range)
        {
            var start = range.Start;
            while (true)
            {
                if (start > range.End)
                    break;

                var end = Math.Min(range.End, RoundToNextHundred(start) - 1);

                yield return new ZipCodeRange(start, end);
                //Console.WriteLine($"{start} - {end}");

                start = end + 1;
            }
        }

        private int RoundToNextHundred(int value)
        {
            return value + (100 - (value % 100));
        }



        int lineIndex = 0;

        private void Process(ReadOnlySequence<byte> line)
        {

            if (lineIndex > 0)
            {
                var originStart = Converter.GetInt32(line.Slice(0, 5));
                var originEnd = Converter.GetInt32(line.Slice(5, 5));
                var destinationStart = Converter.GetInt32(line.Slice(10, 5));
                var destinationEnd = Converter.GetInt32(line.Slice(15, 5));
                var zone = (byte)Converter.GetInt32(line.Slice(20, 2));
                var type = (byte)Converter.GetInt32(line.Slice(22, 2));

                _list.Add(new ZipCodeException(new ZipCodeRange(originStart, originEnd), new ZipCodeRange(destinationStart, destinationEnd), zone, type));
            }

            lineIndex++;
        }
    }
}
