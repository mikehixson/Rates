using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates.PriorityMail
{
    // These shapes have the same "business logic" (none) which is why they are grouped here. But the rates are based on shape, which is why there are 2 seperate tables.
    // We could also do a "Simple" or "No Business Rules" chart class, then reuse that class for all the chats that dont have any special rules.

    //public class PriorityMailRegionalRateBoxChart : IRateChart<Shipment<RegionalRateBoxA>>, IRateChart<Shipment<RegionalRateBoxB>>
    public class PriorityMailRegionalRateBoxChart<T> : IRateChart<Shipment<T>>  // Not all T's have a weight. If we need to check weight here, then we have to figure out what to do.
    {
        private readonly ITableSingleRow<int> _base;

        public PriorityMailRegionalRateBoxChart(ITableSingleRow<int> @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<T> shipment)
        {
            return _base.GetRate(shipment.GetZone().Zone);
        }
    }
}
