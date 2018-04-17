using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public interface ITableSingleColumn<T>
    {
        decimal GetRate(T value);
    }
}
