using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    //todo: all these write overloads are not necessary
    public class Sheet
    {
        private string[] _values;
        private int _columns;
        private int _rows;

        public Sheet(int columns, int rows)     
        {
            _columns = columns;
            _rows = rows;

            _values = new string[columns * rows];
        }
        
        public void Write(Point target, params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                Write(target.X + i, target.Y, values[i]);
            }
        }

        public void Write(int x, int y, params string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                Write(x + i, y, values[i]);
            }
        }

        public void Write(Point target, int columns, params string[] values)
        {
            var i = 0;
            var y = target.Y;

            while (i < values.Length)
            {
                for (var x = target.X; x < target.X + columns; x++, i++)
                    Write(x, y, values[i]);

                y++;
            }
        }

        public void Write(int x, int y, string value)
        {
            _values[(y * _columns) + x] = value;
        }

        public string[] Read(Range target)
        {
            var values = new string[target.Size];

            var i = 0;
            for (var y = target.P1.Y; y < target.P1.Y + target.Rows; y++)
            {
                for (var x = target.P1.X; x < target.P1.X + target.Columns; x++, i++)
                    values[i] = Read(x, y);
            }

            return values;
        }

        public string Read(int x, int y)
        {
            return _values[(y * _columns) + x];
        }

        public void Load(string filePath)
        {
            using (var reader = new CsvHelper.CsvReader(File.OpenText(filePath), new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = false }))
            {
                while (reader.Read())
                {
                    for (var x = 0; x < reader.CurrentRecord.Length && x < _columns; x++)
                        Write(x, reader.Row - 1, reader.CurrentRecord[x].Trim());
                }
            }
        }

        public void Save(string filePath)
        {
            using (var writer = new CsvHelper.CsvWriter(new StreamWriter(filePath), new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = false }))
            {
                for (var y = 0; y < _rows; y++)
                {
                    for (var x = 0; x < _columns; x++)
                        writer.WriteField(Read(x, y));

                    writer.NextRecord();
                }
            }
        }

        public IEnumerable<string> EnumerateValues()
        {
            return _values;
        }
    }
}
