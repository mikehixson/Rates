using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using UspsRates.FirstClassMail;
using Xunit;

namespace UspsRatesTest
{
    public class FirstClassMailTests
    {
        [Fact]
        public void Test()
        {
            var letters = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\FCM Retail Letters.dat");
            var flats = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\FCM Retail Flats.dat");
            var parcels = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\FCM Retail Parcels.dat");
            var postcards = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\FCM Retail Postcards.dat");
            var comParcels = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\FCM Commercial Parcels.dat");


            var mc1 = MyActivator.Get<FirstClassLetterChart>(letters);
            var mc2 = MyActivator.Get<FirstClassFlatChart>(flats);
            var mc3 = MyActivator.Get<FirstClassParcelChart>(parcels);
            var mc4 = MyActivator.Get<FlatRateChart<Postcard>>(postcards);
            var mc5 = MyActivator.Get<FirstClassParcelChart>(comParcels);



            var machinable = new Shipment<Letter>("97035", "97036", new Letter(Weight.FromOunces(1), true));
            var a = mc1.GetRate(machinable);

            var nonmachinable = new Shipment<Letter>("97035", "97036", new Letter(Weight.FromOunces(1), false));
            var b = mc1.GetRate(nonmachinable);

            var c = mc2.GetRate(new Shipment<Flat>("97035", "97036", new Flat(Weight.FromOunces(1))));
            var d = mc3.GetRate(new Shipment<Parcel>("97035", "97036", new Parcel(Weight.FromOunces(1), Dimensions.FromInches(6, 6, 6))));
            var e = mc4.GetRate(new Shipment<Postcard>("97035", "97036", new Postcard()));
            var f = mc5.GetRate(new Shipment<Parcel>("97035", "97036", new Parcel(Weight.FromOunces(1), Dimensions.FromInches(6, 6, 6))));

        }
    }
}
