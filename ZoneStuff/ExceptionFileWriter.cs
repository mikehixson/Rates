using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ZoneStuff
{    
    public class ExceptionFileWriter
    {
        public ZipCodeException[] GetExceptions()
        {
            var filePath = @"C:\Users\Mike\Downloads\charts\exception.txt";

            var records = FileObserver.AllLines(filePath, Process)
                .Skip(1)
                .ToEnumerable()
                .ToArray();

            return records;
        }

        public int[] GetValidZipThree()
        {
            var filePath = @"C:\Users\Mike\Downloads\charts\valid-zip3.txt";

            var records = FileObserver.AllLines(filePath, ProcessValidZipThree)
                .ToEnumerable()
                .ToArray();

            return records;
        }

        public void Run()
        {
            var records = GetExceptions();

            using (var writer = new StreamWriter(@"C:\Users\Mike\Downloads\charts\exception-compressed2.txt"))
            {
                writer.Write("08012018\r\n");

                var compressed = records
                    .CompressByOrigin()
                    .CompressByDestination()
                    .Select(c => $"{c.Origin.Start:00000}{c.Origin.End:00000}{c.Destination.Start:00000}{c.Destination.End:00000}{c.Zone:00}{c.MailType:00}");

                foreach (var line in compressed)
                    writer.Write($"{line}\r\n");
            }

            using (var writer = new StreamWriter(@"C:\Users\Mike\Downloads\charts\exception-expanded2.txt"))
            {
                writer.Write("08012018\r\n");

                var compressed = records
                    .CompressByOrigin()
                    .CompressByDestination()
                    .Expand()
                    .Select(c => $"{c.Origin.Start:00000}{c.Origin.End:00000}{c.Destination.Start:00000}{c.Destination.End:00000}{c.Zone:00}{c.MailType:00}");

                foreach (var line in compressed)
                    writer.Write($"{line}\r\n");
            }


            var valid = GetValidZipThree();

            using (var writer = new StreamWriter(@"C:\Users\Mike\Downloads\charts\exception-expanded3.txt"))
            {
                writer.Write("08012018\r\n");

                var compressed = records
                    .CompressByOrigin()
                    .CompressByDestination()
                    .Expand()
                    .Where(c => valid.Contains(c.Origin.Start / 100) && valid.Contains(c.Destination.Start / 100))
                    .Select(c => $"{c.Origin.Start:00000}{c.Origin.End:00000}{c.Destination.Start:00000}{c.Destination.End:00000}{c.Zone:00}{c.MailType:00}");

                foreach (var line in compressed)
                    writer.Write($"{line}\r\n");
            }
        }                     



        int lineIndex = 0;

        private ZipCodeException Process(ReadOnlySequence<byte> line)
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

            lineIndex++;

            return new ZipCodeException();
        }

        private int ProcessValidZipThree(ReadOnlySequence<byte> line)
        {
            return Converter.GetInt32(line);    
        }
    }
}
