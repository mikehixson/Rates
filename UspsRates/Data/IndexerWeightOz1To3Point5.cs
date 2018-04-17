using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerWeightOz1To3Point5 : IIndexer<Weight>
    {
        public int GetIndex(Weight weight)
        {
            var ounces = weight.TotalOunces;

            if (ounces > 3.5M)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight exceeds maximum.");

            if (ounces <= 3)
                return (int)Math.Ceiling(ounces) - 1;

            return 3;
        }
    }
}
