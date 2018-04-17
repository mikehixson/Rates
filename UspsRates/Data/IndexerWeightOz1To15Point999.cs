using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerWeightOz1To15Point999 : IIndexer<Weight>
    {
        public int GetIndex(Weight weight)
        {
            var ounces = weight.TotalOunces;

            if (ounces >= 16)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight exceeds maximum.");

            if (ounces <= 15)
                return (int)Math.Ceiling(ounces) - 1;

            return 15;
        }
    }
}
