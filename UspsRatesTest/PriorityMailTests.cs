using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates;
using UspsRates.PriorityMail;
using Xunit;

namespace UspsRatesTest
{
    public class PriorityMailTests
    {
        [Fact]
        public void Test2()
        {
            //var data = Substitute.For<ITableData>();
            //var chart = new PriorityMailZoneChart(data);

            //var a = chart.Get(Weight.FromPounds(0.3M), 1);
            //data.Received().Get(0, 0);

            //var a2 = chart.Get(Weight.FromPounds(1.5M), 2);
            //data.Received().Get(0, 1);

            //var a3 = chart.Get(Weight.FromPounds(2.3M), 3);
            //data.Received().Get(1, 2);

            //var b = chart.Get(Weight.FromPounds(1M), 1);
            //data.Received().Get(0, 0);

            //var c = chart.Get(Weight.FromPounds(70M), 9);
            //data.Received().Get(7, 69);
        }

        [Fact]
        public void Balloon()
        {            
            var weight = Weight.FromPounds(1);
            var dimensions = new Dimensions(Distance.FromInches(60), Distance.FromInches(10), Distance.FromInches(5));

            var data = Substitute.For<ITableSet>();
            //data.Columns.Returns((short)8);
            //data.Rows.Returns((short)71);

            //var chart = new PriorityMailZoneChart(new PmData(data));

            //chart.GetRate(new Shipment<Parcel>("97035", "17035", new Parcel(weight, dimensions)));

            // 71st row should be accessed
            //data.Received().Read(Arg.Any<int>(), 71 - 1);
        }

        [Fact]
        public void DimensionalWeight()
        {
            var weight = Weight.FromPounds(1);
            var dimensions = new Dimensions(Distance.FromInches(12), Distance.FromInches(12), Distance.FromInches(13));

            var data = Substitute.For<ITableSet>();  
            //data.Columns.Returns((short)8);
            //data.Rows.Returns((short)71);

            //var chart = new PriorityMailZoneChart(new PmData(data));

            //chart.GetRate(new Shipment<Parcel>("97035", "67035", new Parcel(weight, dimensions)));

            // Row at index 9 should be accessed for the dimensional weight of 1872 / 194 = 9.64; Rounded up to 10
            //data.Received().Read(Arg.Any<int>(), 9);
        }


        [Fact]
        public void Test()
        {
            TableSet set = new TableSet();
            //set.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Letters, Flats & Parcels.dat");
            //set.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Base Letters, Flats & Parcels.dat");
            set.Load(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Plus Letters, Flats & Parcels.dat");


            // Retail 1 - 70
            // Commercial Base 1 - 70
            // Commercial Plus 0.5 - 70

            var mc = MyActivator.Get<PriorityMailZoneChart2>(set);

            var a = mc.GetRate(new Shipment<Letter>("97035", "97036", new Letter(Weight.FromOunces(1))));
            var b = mc.GetRate(new Shipment<Flat>("97035", "97036", new Flat(Weight.FromOunces(1), Dimensions.FromInches(1, 1, 2))));
            var c = mc.GetRate(new Shipment<Parcel>("97035", "97036", new Parcel(Weight.FromPounds(0.7M), new Dimensions(Distance.FromInches(6), Distance.FromInches(6), Distance.FromInches(6)))));
            var d = mc.GetRate(new Shipment<Parcel>("97035", "17036", new Parcel(Weight.FromPounds(0.7M), new Dimensions(Distance.FromInches(60), Distance.FromInches(10), Distance.FromInches(5)))));
        }

        [Fact]
        public void TestFlatRate()
        {
            TableSet set1 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Flat Rate Envelopes.dat");
            TableSet set2 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Legal Flat Rate Envelopes.dat");
            TableSet set3 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Padded Flat Rate Envelopes.dat");
            TableSet set4 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Small Flat Rate Boxes.dat");
            TableSet set5 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Medium Flat Rate Boxes.dat");
            TableSet set6 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail Large Flat Rate Boxes.dat");
            TableSet set7 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Retail APO, FPO & DPO Large Flat Rate Boxes.dat");

            var mc1 = MyActivator.Get<FlatRateChart<FlatRateEnvelope>>(set1);
            var mc2 = MyActivator.Get<FlatRateChart<LegalFlatRateEnvelope>>(set2);
            var mc3 = MyActivator.Get<FlatRateChart<PaddedFlatRateEnvelope>>(set3);
            var mc4 = MyActivator.Get<FlatRateChart<SmallFlatRateBox>>(set4);
            var mc5 = MyActivator.Get<FlatRateChart<MediumFlatRateBox>>(set5);
            var mc6 = MyActivator.Get<FlatRateChart<LargeFlatRateBox>>(set6);
            var mc7 = MyActivator.Get<FlatRateChart<ApoFpoDpoLargeFlatRateBox>>(set7);

            var a = mc1.GetRate(new Shipment<FlatRateEnvelope>("97035", "97036", new FlatRateEnvelope()));
            var b = mc2.GetRate(new Shipment<LegalFlatRateEnvelope>("97035", "97036", new LegalFlatRateEnvelope()));
            var c = mc3.GetRate(new Shipment<PaddedFlatRateEnvelope>("97035", "97036", new PaddedFlatRateEnvelope()));
            var d = mc4.GetRate(new Shipment<SmallFlatRateBox>("97035", "97036", new SmallFlatRateBox()));
            var e = mc5.GetRate(new Shipment<MediumFlatRateBox>("97035", "97036", new MediumFlatRateBox()));
            var f = mc6.GetRate(new Shipment<LargeFlatRateBox>("97035", "97036", new LargeFlatRateBox()));
            var g = mc7.GetRate(new Shipment<ApoFpoDpoLargeFlatRateBox>("97035", "97036", new ApoFpoDpoLargeFlatRateBox()));
        }

        [Fact]
        public void TestRegionalRateBox()
        {
            TableSet set1 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Regional Rate Box A.dat");
            TableSet set2 = TableSet.Create(@"C:\Users\Mike\Google Drive\USPS Rates\2017-01 Rate Files\PM Commercial Regional Rate Box B.dat");

            var mc1 = MyActivator.Get<PriorityMailRegionalRateBoxChart<RegionalRateBoxA>>(set1);
            var mc2 = MyActivator.Get<PriorityMailRegionalRateBoxChart<RegionalRateBoxB>>(set2);

            var a = mc1.GetRate(new Shipment<RegionalRateBoxA>("97035", "97036", new RegionalRateBoxA(Weight.FromPounds(6))));
            var b = mc2.GetRate(new Shipment<RegionalRateBoxB>("97035", "97036", new RegionalRateBoxB(Weight.FromPounds(6))));
        }
    }
}
