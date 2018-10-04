using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;


namespace ZoneStuff
{
    public class MyReader
    {
        private static Zone[] zones = new Zone[1000 * 1000];


        static ZoneFactory _factory = new ZoneFactory();
        static int lineIndex = 0;
        public static void ProcessLine(ReadOnlySequence<byte> line)
        {
            if (lineIndex != 0)
            {
                //var y = GetAsciiString(line);
                //var x = y.Length;
                //Console.WriteLine(x);

                                
                var origin = GetInt32(line.Slice(0, 3));
                var originIndex = Class1.OriginIndex(origin);

                line = line.Slice(3);

                var destination = 1;
                while(line.Length != 0)
                {                    
                    var zone = _factory.Get(line.Slice(0, 2));
                    //var t = GetAsciiString(val);


                    var index = (originIndex * 1000) + Class1.DestinationIndex(destination);
                    

                    zones[index] = zone;

                    destination++;

                    line = line.Slice(2);
                    
            
                }
            }

            lineIndex++;
        }


        // This processing is like data being pused to us. We rather read data similar to reader.ReadLine(). This approach has the advantage of being able to process 
        // multiple lines from one read from the pipe.
        public static async Task Pushed()
        {
            var reader = Go();
            
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                var buffer = result.Buffer;

                if (buffer.IsEmpty && result.IsCompleted)
                    break;

                SequencePosition? position = null;

                //MemoryMarshal.TryGetArray()
                //Utf8Parser.TryParse()

                while ((position = buffer.PositionOf((byte)'\n')) != null)
                {
                    var line = buffer.Slice(0, position.Value);
                    line = line.Slice(0, line.Length - 1);

                    //var l = GetAsciiString(line);


                    // process line
                    ProcessLine(line);
                    //var origin = GetAsciiString(line.Slice(0, 3));
                    //var origin = GetInt32(line.Slice(0, 3));
                    //var o = Utf8Parser.TryParse(line.Slice(0, 3).);

                    //var y = Int32.Parse(origin);

                    // Skip past part of buffer already read. "buffer" now begins at the byte after the new line char
                    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                }

                reader.AdvanceTo(buffer.Start, buffer.End);
            }
        }

        public static string GetAsciiString(ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsSingleSegment)
            {
                return Encoding.ASCII.GetString(buffer.First.Span);
            }

            return string.Create((int)buffer.Length, buffer, (span, sequence) =>
            {
                foreach (var segment in sequence)
                {
                    Encoding.ASCII.GetChars(segment.Span, span);

                    span = span.Slice(segment.Length);
                }
            });
        }


        public static int GetInt32(ReadOnlySequence<byte> buffer)
        {
            int number = 0;

            if (buffer.IsSingleSegment)
            {
                var span = buffer.First.Span;

                for (var i = 0; i < span.Length; i++)
                {
                    var digit = span[i];

                    // Ignore spaces
                    if (digit == 0x20)
                        continue;

                    if (digit < 0x30 || digit > 0x39)
                        throw new Exception("Sequences contains non-numeric characters.");

                    number = (number * 10) + (digit - 0x30);
                }
            }
            else
            {
                foreach (var segment in buffer)
                {
                    var span = segment.Span;

                    for (var i = 0; i < span.Length; i++)
                    {
                        var digit = span[i];

                        // Ignore spaces
                        if (digit == 0x20)
                            continue;

                        if (digit < 0x30 || digit > 0x39)
                            throw new Exception("Sequences contains non-numeric characters.");

                        number = (number * 10) + (digit - 0x30);
                    }
                }
            }

            return number;
        }
               

        //static int GetInt32(ReadOnlySequence<byte> buffer)
        //{
        //    if (buffer.IsSingleSegment)
        //        return BinaryPrimitives.ReadInt32LittleEndian(buffer.First.Span);

        //    return BitConverter.ToInt32(buffer.ToArray());
        //}

        static byte GetValue(ReadOnlySequence<byte> buffer)
        {
            var a = buffer.Slice(0, 1).First.Span[0];
            buffer = buffer.Slice(1);

            var b = buffer.Slice(0, 1).First.Span[0];

            var k = (a - 48);

            switch(b)
            {
                case 0x20: // space
                case 0x31: // 1
                case 0x65: // e
                    break;

                case 0x2A: // *
                case 0x61: // a
                case 0x62: // b
                
                    k |= (1 << 4);
                    break;

                default:
                    throw new Exception();
            }

            return (byte)k;
        }

        public static PipeReader Go()
        {
            var path = @"C:\Users\Mike\Downloads\charts\format2.txt";

            var pipe = new Pipe();
            _ = FillPipeAsync(pipe.Writer, path);

            return pipe.Reader;
        }

        public static async Task FillPipeAsync(PipeWriter writer, string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1, true))
            {
                while (true)
                {
                    var memory = writer.GetMemory();

                    var read = await file.ReadAsync(memory);

                    if (read == 0)
                        break;

                    writer.Advance(read);

                    await writer.FlushAsync();
                }
            }

            writer.Complete();
        }
    }
}
