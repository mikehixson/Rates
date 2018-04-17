using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parsers
{
    public static class DataFileWriter
    {
        public static void Run()
        {
            var binder = new TypeNameSerializationBinder("Parsers.{0}, Parsers");

            var index = JsonConvert.DeserializeObject<ChartData[]>(File.ReadAllText(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 index.json"), new JsonSerializerSettings { Binder = binder, TypeNameHandling = TypeNameHandling.Auto });


            var sourceFolderPath = @"C:\Users\Mike\Google Drive\USPS Rates\January 2017 - Price Files - CSV";
            var targetFolderPath = @"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files";


            foreach (var chart in index)
            {
                var book = new Book();

                foreach (var source in chart.Sources)
                {
                    var input = new Sheet(20, 100);
                    input.Load(Path.Combine(sourceFolderPath, source.Name));

                    book.Sheets.Add(input);

                    if (source.Asserts != null)
                    {
                        foreach (var assert in source.Asserts)
                            assert.Assert(input);
                    }
                }

                using (var stream = File.OpenWrite(Path.Combine(targetFolderPath, chart.Name)))
                using (var writer = new BinaryWriter(stream))
                {

                    var outBook = new Book();

                    foreach (var target in chart.Targets)
                    {

                        var sheet = new Sheet(target.Columns, target.Rows);
                        outBook.Sheets.Add(sheet);

                        foreach (var range in target.Ranges)
                        {
                            range.Write(book, sheet);
                        }

                        //todo: assert columsn and rows are all filled up
                        Write2(writer, target.Strategy.Class, target.Strategy.ColumnIndex, target.Strategy.RowIndex, target.Columns, target.Rows, sheet.EnumerateValues());
                    }
                }
            }
        }



        private static void Write(BinaryWriter writer, short columns, short rows, IEnumerable<string> values)
        {
            var regex = new Regex("[\\d\\.]+");

            //todo: file format version
            writer.Write(columns);
            writer.Write(rows);

            foreach (var value in values)
            {
                var cleanValue = regex.Match(value).Value;

                if (String.IsNullOrWhiteSpace(cleanValue))
                    writer.Write(0);
                else
                    writer.Write((int)(Decimal.Parse(cleanValue) * 1000));
            }

        }

        private static void Write2(BinaryWriter writer, string table, string col, string row, short columns, short rows, IEnumerable<string> values)
        {
            var regex = new Regex("[\\d\\.]+");

            //todo: file format version?
            writer.Write(table);    //todo: make sure classes exist
            writer.Write(col ?? String.Empty);
            writer.Write(row ?? String.Empty);
            writer.Write(columns);
            writer.Write(rows);

            foreach (var value in values)
            {
                var cleanValue = regex.Match(value).Value;

                if (String.IsNullOrWhiteSpace(cleanValue))
                    writer.Write(0);
                else
                    writer.Write((int)(Decimal.Parse(cleanValue) * 1000));
            }

        }
    }
}
