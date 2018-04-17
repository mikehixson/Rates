using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    // RULE: rates are always a function of a shipment

    public interface IShipment
    {
        string Origin { get; }
        string Destination { get; }
        decimal GetRate();
    }


    public class Shipment<T> : IShipment
    {
        public string Origin { get; }
        public string Destination { get; }
        public T Piece;     // How do we get this to be private? do we want that? It shouldnt have to be public is the important point.

        private IRateChart<Shipment<T>> _chart;
        

        public Shipment(string origin, string destination, T piece)
        {
            Origin = origin;
            Destination = destination;
            Piece = piece;
        }

        public Shipment(string origin, string destination, T piece, IRateChart<Shipment<T>> chart) //
        {
            Origin = origin;
            Destination = destination;
            Piece = piece;

            _chart = chart;
        }

        public Shipment(string origin, string destination, T piece, Action chart) //
        {
            Origin = origin;
            Destination = destination;
            Piece = piece;            
        }

        public ZoneDetail GetZone()
        {
            var z = new DummyZoneChart();
            return z.Get(Origin, Destination);
        }

        public decimal GetRate()
        {
            return _chart.GetRate(this);
        }     
    }

    //public static class Extensions
    //{
    //    public static decimal GetRate(this Shipment<Parcel> shipment, IZoneChart<Parcel> rater)
    //    {
    //        return rater.GetRate(shipment);
    //    }
    //}
}
