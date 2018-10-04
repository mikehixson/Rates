using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZoneStuff;
using System.Reactive.Linq;
using System.Buffers;
using System.Linq;
using System.Diagnostics;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Thread.Sleep(3000);
            //ZoneStuff.Class1.Zone(@"C:\Users\Mike\Downloads\charts\format2.txt");
            //ZoneStuff.MyReader.Pushed().Wait();
            var p = new ZoneStuff.ExceptionFileAnalyzer();
            await p.Foo();

            var z = new ZoneStuff.ZoneAnalyzer();
            //z.Run();

            return;

            await DrawEm();

            return;

            var r = new ExceptionFileWriter();
            r.Run();

            return;

            BruteForce();
            Indexed();
            Hashed();

            //return;

            //var list = new List<long>();

            //var o = FileObserver.AllLines(@"C:\Users\Mike\Downloads\charts\exception.txt", Foo)
            //    .Skip(1)
            //    .GroupBy(e => e.Zone)
            //    //.Select(Foo)                
            //    .Subscribe(
            //        l => l.Count().Subscribe(count => Console.WriteLine(count), () => Console.WriteLine($"Done with {l.Key}")),
            //        () => Console.WriteLine("Done")
            //    );

            //Console.ReadLine();


            //exception.txt
            //exception-compressed.txt
            //exception-expanded.txt



        }

        static async Task DrawEm()
        {
            var p = new ZoneStuff.ExceptionFileAnalyzer();
            await p.Foo();
            var groups = p.OriginDestinationPairsThatHaveExceptions().Where(e => e.Count(n => n.MailType == 0) > 0);

            foreach (var group in groups)
            {
                var origin = Int32.Parse(group.Key.Substring(0, 3));
                var destination = Int32.Parse(group.Key.Substring(3, 3));

                //var calculator = new BruteForceZoneCalculator();
                //var exceptions = calculator.ListExceptions(origin, destination);

                var originAdjust = origin * 100;
                var destinationAdjust = destination * 100;

                var exceptions = group.Where(e => e.MailType == 0).Select(e => (e.Destination.Start - destinationAdjust, e.Destination.End - destinationAdjust, e.Origin.Start - originAdjust, e.Origin.End - originAdjust, (int)e.Zone));

                if (exceptions.Count() > 1)
                {
                    Draw.DoIt($@".\images\{origin:000}{destination:000}.png", exceptions);
                    break;
                }
            }
        }

        static void BruteForce()
        {
            Console.WriteLine("BruteForce");

            var calculator = new BruteForceZoneCalculator();

            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("timing");
            var t1 = calculator.Lookup(09749, 86576);
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var t2 = calculator.Lookup(97035, 94040);
            Console.WriteLine(sw.ElapsedTicks);


            Console.WriteLine(t1);
            calculator.ListExceptions(097, 969);
        }

        static void Indexed()
        {
            Console.WriteLine("Indexed");

            var calculator = new IndexedZoneCalculator();

            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("timing");
            var t1 = calculator.Lookup(09749, 86576);
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var t2 = calculator.Lookup(97035, 94040);
            Console.WriteLine(sw.ElapsedTicks);
        }

        static void Hashed()
        {
            Console.WriteLine("Hashed");

            var calculator = new HashedZoneCalculator();

            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("timing");
            var t1 = calculator.Lookup(09749, 86576);
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var t2 = calculator.Lookup(97035, 94040);
            Console.WriteLine(sw.ElapsedTicks);
        }

    }
}
