using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates.FirstClassMail
{
    public class FirstClassLetterChart : IRateChart<Shipment<Letter>>
    {
        private readonly ITableSingleColumn<Weight> _base;
        private readonly ITableSingleCell _nonmachinable;

        public FirstClassLetterChart(ITableSingleColumn<Weight> @base, ITableSingleCell nonmachinable)
        {
            _base = @base;
            _nonmachinable = nonmachinable;
        }

        public decimal GetRate(Shipment<Letter> shipment)
        {
            var rate = _base.GetRate(shipment.Piece.Weight);
            
            if (!shipment.Piece.Machinable)
                rate += _nonmachinable.GetRate();

            return rate;
        }
    }
}
