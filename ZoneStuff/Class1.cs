using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// 0 in the zone chart means dest zip doesnt exist
namespace ZoneStuff
{
    public class Zone
    {
        public int Number { get; private set; }
        public bool Ndc { get; private set; }

        public Zone(int number, bool ndc)
        {
            Number = number;
            Ndc = ndc;
        }

        public override string ToString()
        {
            return $"{Number}, {Ndc}";
        }
    }

    public class ExceptionPair
    {
        public ZipCodeRange Origin { get; private set; }
        public ZipCodeRange Destination { get; private set; }
        public Zone Zone { get; private set; }

        public ExceptionPair(ZipCodeRange origin, ZipCodeRange destination, Zone zone)
        {
            Origin = origin;
            Destination = destination;
            Zone = zone;
        }

        public bool Contains(string origin, string destination)
        {
            return Origin.Contains(origin) && Destination.Contains(destination);
        }
    }

    public class ZipCodeRange
    {
        public string Start { get; private set; }
        public string End { get; private set; }

        public ZipCodeRange(string start, string end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(string zip)
        {
            var z = Int32.Parse(zip);

            return z >= Int32.Parse(Start) && z <= Int32.Parse(End); // Bad performance
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ZipCodeRange);
        }


    }

    public class Cell
    {
        public Zone Zone { get; private set; }
        public List<ExceptionPair> FiveDigitException { get; private set; }
        public List<ExceptionPair> MilitaryException { get; private set; }  // do we have exceptions for both? probably not just need 1 range. nned to check

        public Cell(Zone zone, ExceptionPair fiveDigit, ExceptionPair military)
        {
            Zone = zone;
            FiveDigitException = new List<ExceptionPair>();
            MilitaryException = new List<ExceptionPair>();
        }

        public Zone GetIt(string origin, string destination)
        {
            var fiveException = FiveDigitException.SingleOrDefault(e => e.Contains(origin, destination));

            if (fiveException != null)
                return fiveException.Zone;

            var militaryException = MilitaryException.SingleOrDefault(e => e.Contains(origin, destination));

            if (militaryException != null)
                return militaryException.Zone;

            return Zone;
        }

        public Cell AddFiveDigitException(ExceptionPair exception)
        {
            //if (FiveDigitException != null)
            //throw new Exception("Already have a 5 digit.");

            //return new Cell(Zone, exception, MilitaryException);

            FiveDigitException.Add(exception);

            return this;
        }

        public Cell AddMilitaryException(ExceptionPair exception)
        {
            //if (MilitaryException != null)
            //throw new Exception("Already have a military.");

            //return new Cell(Zone, FiveDigitException, exception);

            MilitaryException.Add(exception);

            return this;
        }
    }


    

    public class Class1
    {
        private static Zone[] zones = new Zone[1000 * 1000];
        private static Cell[] cells = new Cell[1000 * 1000];

        public static void Play()
        {
            var y = cells.Where(c => c != null && c.FiveDigitException.Any()).Select(c => c.FiveDigitException.Count);

            var z = cells.Where(c => c != null && c.FiveDigitException.Any() && c.MilitaryException.Any()).Select(c => new { Five = c.FiveDigitException.Count, Military = c.MilitaryException.Count });

            var a = GetIt("97035", "09000");
        }

        public static Zone GetIt(string origin, string destination)
        {
            var o = Int32.Parse(origin.Substring(0, 3));
            var d = Int32.Parse(destination.Substring(0, 3));

            var index = OriginIndex(o) * 1000 + DestinationIndex(d);
            var item = cells[index];

            if (item == null)
                throw new Exception("Invalid zip");

            return item.GetIt(origin, destination);
            
        }

        public static void Zone(string filePath)
        {


            // we really only have 9 * 2 Zone (answers) to the Get zone question

            var factory = new ZoneFactory();

            var d = new Dictionary<string, Zone>
            {
                { "1 ", new Zone(1, false) },
                { "1*", new Zone(1, true) },
                { "11", new Zone(1, false) },
                { "1a", new Zone(1, true) },
                { "1e", new Zone(1, false) },
                { "1b", new Zone(1, true) },
            };

                       

            using (var reader = new StreamReader(filePath))
            {
                // Date
                reader.ReadLine();



                

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var origin = Int32.Parse(line.Substring(0, 3));

                    for (var i = 3; i < line.Length; i += 2)
                    {
                        var destination = ((i - 3) / 2) + 1;
                        //var zone = new Zone(line[i] - 48, false);   //not always false
                        //var zone = 0;
                        var zone = factory.Get(line.Substring(i, 2));
                        var index = (OriginIndex(origin) * 1000) + DestinationIndex(destination);



                        //zones[index] = zone;
                        cells[index] = new Cell(zone, null, null);
                    }

                }
            }
        }


        public static void Exception(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                // Date
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var originStart = line.Substring(0, 5);
                    var originEnd = line.Substring(5, 5);
                    var destinationStart = line.Substring(10, 5);
                    var destinationEnd = line.Substring(15, 5);
                    var zone = line.Substring(20, 2);
                    var type = line.Substring(22, 2);


                    var o1 = Int32.Parse(originStart.Substring(0, 3));
                    var o2 = Int32.Parse(originEnd.Substring(0, 3));

                    var d1 = Int32.Parse(destinationStart.Substring(0, 3));
                    var d2 = Int32.Parse(destinationEnd.Substring(0, 3));

                    var exception = new ExceptionPair(
                        new ZipCodeRange(originStart, originEnd),
                        new ZipCodeRange(destinationStart, destinationEnd), 
                        new Zone(Byte.Parse(zone), false)); // not always false

                    for (var o = o1; o <= o2; o++)
                    {
                        //if (o == 99 || o == 213 || o == 269 || o == 343)
                        //    continue;   //wtf!?

                        var originIndex = OriginIndex(o);

                        if (originIndex < 0)
                        {
                            Console.WriteLine($"Exception with origin {o}");
                            continue;
                        }

                        for (var d = d1; d <= d2; d++)
                        {
                            var destinationIndex = DestinationIndex(d);

                            if (destinationIndex < 0)
                            {
                                Console.WriteLine($"Exception with destination {o}");
                                continue;
                            }

                            var index = (originIndex * 1000) + destinationIndex;
                            var item = cells[index];

                            if (item == null)       //for now
                                continue;

                            if (type == "01")
                                cells[index] = item.AddMilitaryException(exception);
                            else
                                cells[index] = item.AddFiveDigitException(exception);
                        }
                    }
                }
            }
        }

        public static int OriginIndex(int origin)
        {
            // Origin starts at 005
            return origin - 5;  // exceptions have 001
        }

        public static int DestinationIndex(int destination)
        {
            // Destination stats at 001
            return destination - 1;
        }

   
    }

 
}
