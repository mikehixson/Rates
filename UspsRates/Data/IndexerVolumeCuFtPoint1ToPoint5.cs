using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class IndexerVolumeCuFtPoint1ToPoint5 : IIndexer<decimal>
    {
        public int GetIndex(decimal value)
        {
            if (value > 0.5M)
                throw new ArgumentOutOfRangeException(nameof(value), "Value exceeds maximum.");

            return (int)Math.Ceiling(value * 10) - 1;
        }
    }
}
