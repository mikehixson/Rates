using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace ZoneStuff
{
    public class ZoneFactory
    {
        private const byte NumberOfZones = 10;  // what to do about zone 0?
        private const string FalseSymbols = " 1e";
        private const string TrueSymbols = "*ab";

        private Zone[] _zonesA;
        private Zone[] _zonesB;

        public ZoneFactory()
        {
            _zonesA = new Zone[NumberOfZones];
            _zonesB = new Zone[NumberOfZones];

            for (var i = 0; i < NumberOfZones; i++)     // zones start at 0
            {
                _zonesA[i] = new Zone(i, false);
                _zonesB[i] = new Zone(i, true);
            }
        }

        public Zone Get(string token)
        {
            if (FalseSymbols.Contains(token[1]))
                return _zonesA[token[0] - 48];          // zones start at 0
            else
                return _zonesB[token[0] - 48];          // zones start at 0
        }



        public Zone Get(ReadOnlySequence<byte> buffer)
        {
            var a = buffer.Slice(0, 1).First.Span[0];
            buffer = buffer.Slice(1);

            var b = buffer.Slice(0, 1).First.Span[0];

            if (FalseSymbols.Contains((char)b))
                return _zonesA[a - 48];          // zones start at 0
            else
                return _zonesB[a - 48];          // zones start at 0
        }
    }
}
