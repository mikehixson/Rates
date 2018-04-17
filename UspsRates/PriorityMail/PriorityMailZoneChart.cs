using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates
{
    public class PriorityMailZoneChart : IRateChart<Shipment<Letter>>, IRateChart<Shipment<Flat>>, IRateChart<Shipment<Parcel>> //todo: class name should be related to shapes
    {
        private readonly ITable<int, Weight> _base;
        private readonly ITableSingleRow<int> _balloon;
        
        public PriorityMailZoneChart(ITable<int, Weight> @base, ITableSingleRow<int> balloon)
        {
            _base = @base;
            _balloon = balloon;
        }

        public decimal GetRate(Shipment<Letter> shipment)
        {
            return Get(shipment.GetZone().Zone, shipment.Piece.Weight, null);
        }

        public virtual decimal GetRate(Shipment<Flat> shipment)
        {
            return Get(shipment.GetZone().Zone, shipment.Piece.Weight, null);
        }

        public virtual decimal GetRate(Shipment<Parcel> shipment)
        {
            return Get(shipment.GetZone().Zone, shipment.Piece.Weight, shipment.Piece.Dimensions);
        }

        private decimal Get(int zone, Weight weight, Dimensions dimensions)
        {
            if (dimensions != null) // todo: make this method specific to parcels so we dont need this check?
            {
                if (zone <= 4)
                {
                    // Parcels addressed for delivery to zones 1–4 (including local) that weigh less than 20 pounds but measure more than 84 inches in
                    // combined length and girth(but not more than 108 inches) are charged for a 20 - pound parcel (balloon price) based on the applicable zone.
                    if (weight < Weight.FromPounds(20))
                    {
                        if (dimensions.Length + dimensions.Girth > Distance.FromInches(84))     //todo: is there a limit over 84.. 108?
                            return _balloon.GetRate(zone);
                    }
                }
                else
                {
                    // Parcels addressed for delivery to zones 5–9 that exceed one cubic foot (1,728 cubic inches) are charged based on the actual weight
                    // or the dimensional weight, whichever is greater (as calculated in DMM 123.1.4).
                    if (dimensions.VolumeInCubicFeet > 1)
                    {
                        if (dimensions.DimensionalWeight > weight)
                            return _base.GetRate(zone, dimensions.DimensionalWeight);
                    }
                }
            }

            return _base.GetRate(zone, weight);
        }
    }


    //Commercial Plus cubic prices are not based on weight, but are charged by zone and cubic measurement of the mailpiece with any fraction of a measurement rounded down to the nearest 1/4 inch. For example, if a dimension of a Commercial Plus cubic piece measures 12-3/8 inches, it is rounded down to 12-1/4 inches.
    //mailpiece must measure .50 cubic foot or less, weigh 20 pounds or less, and the longest dimension may not exceed 18 inches. Cubic-priced mailpieces may not be rolls or tubes.

    public class PriorityMailZoneChart2 : PriorityMailZoneChart //todo: do this better, this derrived class doesnt not support letters
    {
        private readonly ITable<int, decimal> _cubic;

        public PriorityMailZoneChart2(ITable<int, Weight> @base, ITableSingleRow<int> balloon, ITable<int, decimal> cubic) 
            : base (@base, balloon)
        {
            _cubic = cubic;
        }

        public override decimal GetRate(Shipment<Flat> shipment)
        {
            var @base = base.GetRate(shipment);
            var cubic = Get(shipment.GetZone().Zone, shipment.Piece.Dimensions.RoundDownToNearest(Distance.FromInches(0.25M)).VolumeInCubicFeet);

            if (cubic != null && cubic < @base)
                return (decimal)cubic;

            return @base;
        }

        public override decimal GetRate(Shipment<Parcel> shipment)
        {
            var @base = base.GetRate(shipment);
            var cubic = Get(shipment.GetZone().Zone, shipment.Piece.Dimensions.RoundDownToNearest(Distance.FromInches(0.25M)).VolumeInCubicFeet);

            if (cubic != null && cubic < @base)
                return (decimal)cubic;

            return @base;
        }

        private decimal? Get(int zone, decimal volume)
        {
            if (volume > 0.5M)      //todo: is returning null something we should always do when out of bounds?     Maybe we take care of this when we do the "CanHandle(shipment)" implementaiton?
                return null;

            return _cubic.GetRate(zone, volume);
        }
    }
}
