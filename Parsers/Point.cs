using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point() { }

        public Point(string label)
        {
            if (label.Length < 2)
                throw new ApplicationException("Invalid label.");

            var letterCount = 0;
            var numberCount = 0;

            for (var i = 0; i < label.Length; i++)
            {
                var c = label[i];

                // Check for ascii
                if (c > 127)
                    throw new ApplicationException("Invalid label.");

                if (Char.IsLetter(c) && i == letterCount)
                {
                    if (Char.IsUpper(c))
                        X = (X * 26) + (c - 64);
                    else
                        X = (X * 26) + (c - 96);

                    letterCount++;
                }
                else if (Char.IsDigit(c) && i > 0 && (c != '0' || numberCount > 0))
                {
                    Y = (Y * 10) + (c - 48);
                    numberCount++;
                }
                else
                {
                    throw new ApplicationException("Invalid label.");
                }
            }

            if (letterCount == 0 || numberCount == 0)
                throw new ApplicationException("Invalid label.");

            X--;
            Y--;
        }
    }

    public class Point2
    {
        public int Sheet { get; }
        public Point Point { get; }

        public Point2(string label)
        {
            var parts = label.Split('|');

            if (parts.Length != 2)
                throw new ApplicationException("Invalid label.");

            Sheet = Int32.Parse(parts[0]);
            Point = new Point(parts[1]);
        }
    }
}
