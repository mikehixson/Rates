using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public interface ITable<TRow, TCol>
    {
        decimal GetRate(TRow zone, TCol weight);
    }
}
