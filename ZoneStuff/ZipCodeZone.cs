using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneStuff
{
    public struct ZipCodeZone
    {
        public short Origin { get; private set; }
        public short Destination { get; private set; }
        public byte Number { get; private set; }
        public byte Code { get; private set; }

        public ZipCodeZone(short origin, short destination, byte number, byte code)
        {
            Origin = origin;
            Destination = destination;
            Number = number;
            Code = code;
        }

        public override string ToString()
        {
            return $"{Origin:000} {Destination:000} {Number} {(char)Code}";
        }
    }
}
