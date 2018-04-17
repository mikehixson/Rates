using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates
{
    public class FlatRateChart<T> : IRateChart<Shipment<T>>
    {
        private readonly ITableSingleCell _base;

        public FlatRateChart(ITableSingleCell @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<T> shipment)
        {
            return _base.GetRate();
        }
    }
}
