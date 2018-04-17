using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates
{
    public class PriorityMailExpressZoneChart : IRateChart<Shipment<Letter>>, IRateChart<Shipment<Flat>>, IRateChart<Shipment<Parcel>>
    {
        private readonly ITable<int, Weight> _base;
        
        public PriorityMailExpressZoneChart(ITable<int, Weight> @base)
        {
            _base = @base;
        }

        public decimal GetRate(Shipment<Letter> shipment)
        {
            return _base.GetRate(shipment.GetZone().Zone, shipment.Piece.Weight);
        }

        public decimal GetRate(Shipment<Flat> shipment)
        {
            return _base.GetRate(shipment.GetZone().Zone, shipment.Piece.Weight);
        }

        public decimal GetRate(Shipment<Parcel> shipment)
        {
            return _base.GetRate(shipment.GetZone().Zone, shipment.Piece.Weight);
        }
    }
}
