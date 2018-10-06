using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace ZoneStuff
{
    public static class Converter
    {
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


        static byte GetValue(ReadOnlySequence<byte> buffer)
        {
            var a = buffer.Slice(0, 1).First.Span[0];
            buffer = buffer.Slice(1);

            var b = buffer.Slice(0, 1).First.Span[0];

            var k = (a - 48);

            switch (b)
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
    }
}
