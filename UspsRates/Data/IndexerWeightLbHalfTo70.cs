using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerWeightLbHalfTo70 : IIndexer<Weight>
    {
        public int GetIndex(Weight weight)
        {
            var pounds = weight.TotalPounds;

            if (pounds > 70)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight exceeds maximum.");

            // First row is 0.5 lbs, Second is 1 lbs, then 1 lbs increments
            if (pounds <= 0.5M)
                return 0;

            return (int)Math.Ceiling(pounds);
        }
    }
}
