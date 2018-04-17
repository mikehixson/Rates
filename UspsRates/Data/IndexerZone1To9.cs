using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerZone1To9 : IIndexer<int>
    {
        public int GetIndex(int zone)
        {
            // 9 zones
            // 1, 2 in first col
            
            if (zone < 1)
                throw new ArgumentException(nameof(zone), "Zone is below minimum.");

            if (zone > 9)
                throw new ArgumentException(nameof(zone), "Zone exceeds maximum.");

            if (zone < 3)
                return 0;

            return zone - 2;
        }
    }
}
