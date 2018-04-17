using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Dynamic;
using Parsers.RateList;
using System.Diagnostics;

namespace Parsers
{
    // CSV Files here: https://pe.usps.com/PriceChange/Index
    class Program
    {
        static void Main(string[] args)
        {
            DataFileWriter.Run();
        }

        private static void Fact()
        {

            var r = Pm.Chart().SelectMany(e => Pm.LengthGirth(e)).SelectMany(e => Pm.DimWeight(e)).ToArray();

            var sw = new Stopwatch();
            sw.Start();

            var n = 1000000;
            for (var i = 0; i < n; i++)
            {

                // All records at a specific weight
                //var q = r.Where(e => e.WeightEquals(15) && e.DimWeightEquals(15));
                // All records at a specific zone
                //var q = r.Where(e => e.ZoneEquals(5));
                // All records at a specific price
                //var q = r.Where(e => e.Value == 70.10M);

                // Chart as it appears in docs
                //var q = r.Where(e => e.LengthGirthGreaterEquals(false) && e.ExceedCubicFootEquals(false));

                // Chart based on DimWeight (Zone 5 - 9)
                //var q = r.Where(e => e.Zone >= 5 && e.ExceedCubicFootEquals(true) && e.DimGreaterEquals(true));

                // Chart based on Length + Girth (Zone 0 - 4)
                //var q = r.Where(e => e.Zone <= 4 && e.Weight <= 19 && e.LengthGirthGreaterEquals(true));

                // Exact Match
                var a = r.FirstOrDefault(e => e.Zone == 5 && e.Weight == 25 && e.ExceedCubicFoot == false);

                //var q = r.Where(e => e.ZoneEquals(5) && (e.WeightEquals(5) || e.DimWeightEquals(5)));
                //var q = r.Where(e => e.ZoneEquals(5) && e.WeightEquals(46) && e.ExceedCubicFootEquals(false));
                //var q = r.Where(e => e.ZoneEquals(5) && e.WeightEquals(46) && e.DimWeightEquals(66));
            }

            sw.Stop();

            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / n);
            Console.WriteLine("{0:n} lookup / sec", (n / sw.Elapsed.TotalMilliseconds) * 1000);

            var q = r.Where(e => e.Zone == 5 && e.Weight == 25 && e.ExceedCubicFoot == false).ToArray();

            Console.WriteLine("{0} / {1}", q.Count(), r.Count());

            foreach (var p in q)
                Console.WriteLine(JsonConvert.SerializeObject(p, Formatting.None));
        }
    }











    public class ChartData
    {
        public string Name { get; set; }
        public Source[] Sources { get; set; }
        public Target[] Targets { get; set; }       
        

        // add versioning
    }

    public class Source
    {
        public string Name { get; set; }
        public IAssertion[] Asserts { get; set; }
    }

    public class Target
    {
        public Strategy Strategy { get; set; }
        public short Columns { get; set; }
        public short Rows { get; set; }
        public ISheetWriter[] Ranges { get; set; }
    }

    public interface ISheetWriter
    {
        void Write(Book input, Sheet sheet);
    }

    public class Table : ISheetWriter
    {
        public string Source { get; set; }
        public string Target { get; set; }

        public void Write(Book input, Sheet sheet)     // todo: construct with source 
        {
            var range = new Range2(Source);     // todo: renaming

            var values = input.Read(range);

            sheet.Write(new Point(Target), range.Range.Columns, values);
        }
    }

    public class Literal : ISheetWriter
    {
        public string[] Source { get; set; }
        public string Target { get; set; }

        public void Write(Book input, Sheet sheet)     // todo: remove input as its not used
        {
            sheet.Write(new Point(Target), Source);
        }
    }

    public interface IAssertion
    {
        void Assert(Sheet input);
    }

    public class Asserter : IAssertion
    {
        public string Range { get; set; }
        public string[] Values { get; set; }

        public void Assert(Sheet input)
        {
            var range = new Range(Range);
            var values = input.Read(range);

            if (!Enumerable.SequenceEqual(values, Values))
            {
                var expected = Values.Aggregate((a, v) => $"{a}, {v}");
                var actual = values.Aggregate((a, v) => $"{a}, {v}");

                throw new Exception($"Expected: {expected}, Actual: {actual}");
            }
        }
    }

    public class Constant : IAssertion
    {
        public string Range { get; set; }
        public string Value { get; set; }

        public void Assert(Sheet input)
        {
            var range = new Range(Range);
            var values = input.Read(range);

            if (!values.All(v => v == Value))
                throw new Exception();
        }
    }

    public class Strategy
    {
        public string Class { get; set; }
        public string ColumnIndex { get; set; }
        public string RowIndex { get; set; }
    }

    public class TypeNameSerializationBinder : SerializationBinder
    {
        public string TypeFormat { get; private set; }

        public TypeNameSerializationBinder(string typeFormat)
        {
            TypeFormat = typeFormat;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            string resolvedTypeName = string.Format(TypeFormat, typeName);

            return Type.GetType(resolvedTypeName, true);
        }
    }
}
