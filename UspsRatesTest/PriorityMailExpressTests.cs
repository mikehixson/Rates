using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using Xunit;

namespace UspsRatesTest
{
    public class PriorityMailExpressTests
    {
        [Fact]
        public void Test()
        {
            TableSet set = new TableSet();
            set.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PME Retail Letters, Flats & Parcels.dat");

            // Commercial Base
            // Commercial Plus

            var mc = MyActivator.Get<PriorityMailExpressZoneChart>(set);

            var a = mc.GetRate(new Shipment<Letter>("97035", "97036", new Letter(Weight.FromPounds(1))));
            var b = mc.GetRate(new Shipment<Flat>("97035", "97036", new Flat(Weight.FromPounds(0.4M))));
            var c = mc.GetRate(new Shipment<Parcel>("97035", "97036", new Parcel(Weight.FromPounds(0.7M), new Dimensions(Distance.FromInches(6), Distance.FromInches(6), Distance.FromInches(6)))));
        }

        [Fact]
        public void TestFlatRate()
        {
            TableSet set1 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PME Commercial Base Flat Rate Envelopes.dat");
            TableSet set2 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PME Commercial Base Legal Flat Rate Envelopes.dat");
            TableSet set3 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PME Commercial Base Padded Flat Rate Envelopes.dat");

            var mc1 = MyActivator.Get<FlatRateChart<FlatRateEnvelope>>(set1);
            var mc2 = MyActivator.Get<FlatRateChart<LegalFlatRateEnvelope>>(set2);
            var mc3 = MyActivator.Get<FlatRateChart<PaddedFlatRateEnvelope>>(set3);

            var a = mc1.GetRate(new Shipment<FlatRateEnvelope>("97035", "97036", new FlatRateEnvelope()));
            var b = mc2.GetRate(new Shipment<LegalFlatRateEnvelope>("97035", "97036", new LegalFlatRateEnvelope()));
            var c = mc3.GetRate(new Shipment<PaddedFlatRateEnvelope>("97035", "97036", new PaddedFlatRateEnvelope()));
        }
    }
}
