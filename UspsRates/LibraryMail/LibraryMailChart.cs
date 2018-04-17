using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates.MediaMail
{
    public class LibraryMailChart : IRateChart<Shipment<Flat>>, IRateChart<Shipment<Parcel>>
    {
        private readonly ITableSingleColumn<Weight> _base;

        public LibraryMailChart(ITableSingleColumn<Weight> @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<Flat> shipment)
        {
            return _base.GetRate(shipment.Piece.Weight);
        }

        public decimal GetRate(Shipment<Parcel> shipment)
        {
            return _base.GetRate(shipment.Piece.Weight);
        }
    }
}
