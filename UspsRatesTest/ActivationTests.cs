using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using Xunit;

namespace UspsRatesTest
{
    public class ActivationTests
    {
        [Fact]
        public void Test()
        {
            var setRetail = new TableSet();
            setRetail.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Letters, Flats & Parcels.dat");

            var setCb = new TableSet();
            setCb.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Base Letters, Flats & Parcels.dat");

            var setCp = new TableSet();
            setCp.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Plus Letters, Flats & Parcels.dat");

            var retail = MyActivator.Get<PriorityMailZoneChart>(setRetail);
            var cb = MyActivator.Get<PriorityMailZoneChart>(setCb);
            var cp = MyActivator.Get<PriorityMailZoneChart2>(setCp);



            //var o = MyActivator.GetIndexer("ZoneWeight", "ZoneLTo9", "WeightLb1To70");

            var letter = new Shipment<Letter>("97035", "97035", new Letter(Weight.FromOunces(1)));

            var a = retail.GetRate(letter);
            var b = cb.GetRate(letter);
            var c = cp.GetRate(letter);
        }
    }
}
