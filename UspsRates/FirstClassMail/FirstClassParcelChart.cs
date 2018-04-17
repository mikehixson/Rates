using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates.FirstClassMail
{
    // irregular surcharge
    // impb noncompliant fee


    public class FirstClassParcelChart : IRateChart<Shipment<Parcel>>
    {
        private readonly ITableSingleColumn<Weight> _base;

        public FirstClassParcelChart(ITableSingleColumn<Weight> @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<Parcel> shipment)
        {
            return _base.GetRate(shipment.Piece.Weight);
        }
    }
}
