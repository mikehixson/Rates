using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public interface IRateChart<T>  //should this be Shipment<T>?
    {
        decimal GetRate(T shipment);
    }
}
