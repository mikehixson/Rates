using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerWeightOz1To13 : IIndexer<Weight>
    {
        public int GetIndex(Weight weight)
        {
            var ounces = weight.TotalOunces;

            if (ounces > 13)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight exceeds maximum.");

            return (int)Math.Ceiling(ounces) - 1;
        }
    }
}
