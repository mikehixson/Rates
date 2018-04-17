using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public interface IZoneLookup
    {
        ZoneDetail Get(string origin, string destination);
    }

    public class DummyZoneChart : IZoneLookup
    {
        public ZoneDetail Get(string origin, string destination)
        {
            var zone = destination[0] - 48;
            var ndc = origin[0] == destination[0];

            // Special AK zip codes
            if (origin == "99555" && destination == "99677")
                zone = 1;

            return new ZoneDetail(zone, ndc);
        }
    }

    public class ZoneDetail
    {
        public int Zone { get; private set; }

        // Origin and Destination are within the same Network Distribution Center (NDC)
        public bool SameNdc { get; private set; }

        public ZoneDetail(int zone, bool sameNdc)
        {
            Zone = zone;
            SameNdc = sameNdc;
        }
    }
}
