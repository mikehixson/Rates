using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.RateList
{
    public class Pm
    {

        private static decimal[] baloon = new[] { 14.02M, 14.02M, 14.02M, 17.17M, 19.73M };

        // Create a record for every cell in a table
        public static IEnumerable<Record> Chart()
        {
            var input = new Sheet(20, 100);
            input.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\!PM Retail Letters, Flats & Parcels.csv");

            for (var y = 1; y < 71; y++)    // skip header row
            {
                var weight = Decimal.Parse(input.Read(0, y));

                for (var x = 1; x < 9; x++) // skip header column
                {
                    var value = input.Read(x, y);

                    if (x == 1)
                    {
                        // First column is zones 0-2
                        for(var n = 0; n < 3; n++)
                            yield return new Record { Weight = weight, Zone = n, Value = Decimal.Parse(value) };
                    }
                    else
                    {
                        var zone = input.Read(x, 0).Last() - 48;                        

                        yield return new Record { Weight = weight, Zone = zone, Value = Decimal.Parse(value) };
                    }
                }
            }
        }

        // Expand set
        public static IEnumerable<Record> DimWeight(Record item)
        {            
            if(item.Zone >= 5 && item.Zone <= 9)    // Implies zone must be setup first
            {
                item.ExceedCubicFoot = false;
                yield return item;

                var item2 = item.Clone();
                item2.DimWeight = item2.Weight;
                item2.Weight = null;
                item2.ExceedCubicFoot = true;
                item2.DimGreater = true;
                yield return item2;

                var item3 = item.Clone();
                item3.ExceedCubicFoot = true;
                item3.DimGreater = false;
                yield return item3;
            }
            else
            {
                yield return item;
            }
        }

        // Expand set
        public static IEnumerable<Record> LengthGirth(Record item)
        {
            if (item.Zone >= 0 && item.Zone <= 4 && item.Weight < 20)   // why did i do it this way instead of having data elements: L+G < && weight < 20 && weight == null
            {
                item.LPlusGGreater = false;
                yield return item;

                var item2 = item.Clone();
                item2.LPlusGGreater = true;
                item2.Value = baloon[(int)item2.Zone];  // Implies zone must be setup first

                yield return item2;
            }
            else
            {
                yield return item;
            }
        }
    }
}
