using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public class Range
    {
        public Point P1 { get; }
        public Point P2 { get; }

        public int Size
        {
            get { return Columns * Rows; }
        }

        public int Columns
        {
            get { return (P2.X - P1.X) + 1; }
        }

        public int Rows
        {
            get { return (P2.Y - P1.Y) + 1; }
        }

        public Range(string label)
        {
            var parts = label.Split(':');

            if(parts.Length != 2)
                throw new ApplicationException("Invalid label.");

            P1 = new Point(parts[0]);
            P2 = new Point(parts[1]);
        }
    }

    public class Range2
    {
        public int Sheet { get; }
        public Range Range { get; }

        public Range2(string label)
        {
            var parts = label.Split('|');

            if (parts.Length != 2)
                throw new ApplicationException("Invalid label.");

            Sheet = Int32.Parse(parts[0]);
            Range = new Range(parts[1]);
        }
    }
}
