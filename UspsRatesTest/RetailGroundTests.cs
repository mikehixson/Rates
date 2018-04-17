using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using Xunit;

namespace UspsRatesTest
{
    public class RetailGroundTests
    {
        [Fact]
        public void Test()
        {
            var set = new TableSet();
            set.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\Retail Ground.dat");

            var mc = MyActivator.Get<RetailGroundZoneChartParcels>(set);

            var baloonParcel = new Parcel(Weight.FromPounds(3), new Dimensions(Distance.FromInches(10), Distance.FromInches(10), Distance.FromInches(50)));

            var balloonStandard = new Shipment<Parcel>("97035", "94040", baloonParcel);
            var a = mc.GetRate(balloonStandard);

            var balloonLor = new Shipment<Parcel>("99555", "99677", baloonParcel);
            var b = mc.GetRate(balloonLor);

        }
    }
}
