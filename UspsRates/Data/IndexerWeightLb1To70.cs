using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerWeightLb1To70 : IIndexer<Weight>
    {
        public int GetIndex(Weight weight)
        {
            var pounds = weight.TotalPounds;

            if (pounds > 70)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight exceeds maximum.");

            return (int)Math.Ceiling(pounds) - 1;
        }
    }
}
