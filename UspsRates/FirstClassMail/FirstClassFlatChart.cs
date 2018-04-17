using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates.FirstClassMail
{

    // Could combine with Parcels (via generic chart class) if Weight was a common property. Not sure if this desireable
    public class FirstClassFlatChart : IRateChart<Shipment<Flat>>
    {
        private readonly ITableSingleColumn<Weight> _base;

        public FirstClassFlatChart(ITableSingleColumn<Weight> @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<Flat> shipment)
        {
            return _base.GetRate(shipment.Piece.Weight);
        }
    }


 



}
