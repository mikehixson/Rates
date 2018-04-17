using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerZoneLTo4 : IIndexer<int>
    {
        public int GetIndex(int zone)
        {
            // 5 zones
            // Local, 1, 2 in first col

            if (zone > 4)
                throw new ArgumentException(nameof(zone), "Zone exceeds maximum.");

            if (zone < 3)
                return 0;

            return zone - 2;
        }
    }
}
