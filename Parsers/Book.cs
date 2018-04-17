using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public class Book
    {
        public List<Sheet> Sheets { get; }  //todo: private make book IEnumerable

        public Book()
        {
            Sheets = new List<Sheet>();
        }

        public void Write(Point2 target, params string[] values)
        {
            Sheets[target.Sheet].Write(target.Point);
        }

        public void Write(int sheet, int x, int y, params string[] values)
        {
            Sheets[sheet].Write(x, y, values);
        }

        public void Write(Point2 target, int columns, params string[] values)
        {
            Sheets[target.Sheet].Write(target.Point, columns, values);
        }

        public void Write(int sheet, int x, int y, string value)
        {
            Sheets[sheet].Write(x, y, value);
        }

        public string[] Read(Range2 target)
        {
            return Sheets[target.Sheet].Read(target.Range);
        }

        public string Read(int sheet, int x, int y)
        {
            return Sheets[sheet].Read(x, y);
        }
    }
}
