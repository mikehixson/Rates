using EasyPost;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EasyPostTests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;

            ClientManager.SetCurrent("Rf6WECBQIHS60VPuDBs0Nw");
        }

        [Fact]
        public void TestMethod1()
        {     

            
            var origin = new Address
            {
                name = "Mike Hixson",
                street1 = "1 Main Street",
                city = "Akiachak",
                state = "AK",
                zip = "99551"
            };

            origin.Create();

            var destination = new Address
            {
                name = "Mike Hixson",
                street1 = "1215 Cowles Street",
                city = "Fairbanks",
                state = "AK",
                zip = "99701"
            };

            destination.Create();

            var parcel = new Parcel
            {
                length = 6,
                width = 6,
                height = 6,
                weight = 140
            };

            var shipment = new Shipment
            {
                from_address = origin,
                to_address = destination,
                parcel = parcel
            };

            shipment.Create();

        }

        [Fact]
        public void Foo()
        {
            
            var shipment = Shipment.Retrieve("shp_4ab54a76fd2b40a7a87d12e72894f2bc");   // AK to AK
            //var shipment = Shipment.Retrieve("shp_ea04c8f9ec044fd3b0eae7875bf085b9");   // LO to SJ

            foreach(var rate in shipment.rates)
            {
                _output.WriteLine($"{rate.carrier} {rate.service}: {rate.rate}");
            }
        }
    }
}
