using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using UspsRates.MediaMail;
using Xunit;

namespace UspsRatesTest
{
    public class MediaMailTests
    {
        [Fact]
        public void Test()
        {
            var set = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\Media Mail Retail Flats & Parcels.dat");

            var mc = MyActivator.Get<MedialMailChart>(set);

            var a = mc.GetRate(new Shipment<Flat>("97035", "94040", new Flat(Weight.FromPounds(3))));
            var b = mc.GetRate(new Shipment<Parcel>("97035", "94040", new Parcel(Weight.FromPounds(5), Dimensions.FromInches(6, 6, 6))));
        }
    }
}
