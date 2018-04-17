using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;

namespace UspsRates
{

    //todo: possibly a new flag "Requires Ground Transporation" For Zone 1-4 restriction
    //todo: what restrictions do we need due to this mail class not having a "Local" zone?

    public class RetailGroundZoneChartParcels : IRateChart<Shipment<Parcel>>
    {
        #region AlaskaZipCodes
        private static readonly HashSet<string> AlaskaZipCodes = new HashSet<string>() { "99545", "99546", "99547", "99548", "99549", "99550", "99551", "99552", "99553", "99554", "99555", "99557", "99558", "99559", "99561", "99563", "99564", "99565", "99569", "99571", "99574", "99575", "99576", "99578", "99579", "99580", "99581", "99583", "99585", "99589", "99590", "99591", "99602", "99604", "99606", "99607", "99608", "99609", "99612", "99613", "99614", "99615", "99619", "99620", "99621", "99622", "99624", "99625", "99626", "99627", "99628", "99630", "99632", "99633", "99634", "99636", "99637", "99638", "99640", "99641", "99643", "99644", "99647", "99648", "99649", "99650", "99651", "99653", "99655", "99656", "99657", "99658", "99659", "99660", "99661", "99662", "99663", "99665", "99666", "99667", "99668", "99670", "99671", "99675", "99677", "99678", "99679", "99680", "99681", "99682", "99684", "99685", "99689", "99690", "99691", "99692", "99695", "99697", "99720", "99721", "99722", "99723", "99724", "99726", "99727", "99730", "99732", "99733", "99734", "99736", "99738", "99739", "99740", "99741", "99742", "99745", "99746", "99747", "99748", "99749", "99750", "99751", "99752", "99753", "99754", "99756", "99757", "99758", "99759", "99761", "99762", "99763", "99765", "99766", "99767", "99768", "99769", "99770", "99771", "99772", "99773", "99774", "99777", "99778", "99781", "99782", "99783", "99784", "99785", "99786", "99788", "99789", "99790", "99791", "99801", "99802", "99803", "99811", "99812", "99820", "99821", "99824", "99825", "99826", "99827", "99829", "99830", "99832", "99833", "99835", "99836", "99840", "99841", "99850", "99901", "99903", "99918", "99919", "99921", "99922", "99923", "99925", "99926", "99927", "99928", "99929", "99950" };
        #endregion

        private readonly InternalChart _standard;
        private readonly InternalChart _lor;

        public RetailGroundZoneChartParcels(ITable<int, Weight> standardBase, ITableSingleRow<int> standardBalloon, ITableSingleRow<int> standardOversized, ITable<int, Weight> lorBase, ITableSingleRow<int> lorBalloon, ITableSingleRow<int> lorOversized)
        {
            _standard = new InternalChart(standardBase, standardBalloon, standardOversized);
            _lor = new InternalChart(lorBase, lorBalloon, lorOversized);
        }

        public decimal GetRate(Shipment<Parcel> shipment)
        {
            var chart = IsIntraAlaska(shipment) ? _lor : _standard;

            return chart.Get(shipment.Piece.Weight, shipment.GetZone().Zone, shipment.Piece.Dimensions);
        }

        private bool IsIntraAlaska(IShipment shipment)
        {
            return AlaskaZipCodes.Contains(shipment.Origin) && AlaskaZipCodes.Contains(shipment.Destination);
        }


        private class InternalChart
        {
            private readonly ITable<int, Weight> _base;
            private readonly ITableSingleRow<int> _balloon;
            private readonly ITableSingleRow<int> _oversized;

            public InternalChart(ITable<int, Weight> @base, ITableSingleRow<int> balloon, ITableSingleRow<int> oversized)
            {
                _base = @base;
                _balloon = balloon;
                _oversized = oversized;
            }

            public decimal Get(Weight weight, int zone, Dimensions dimensions = null)      //todo: make dimensions required... caller can bypass with a 0 length
            {
                if (dimensions != null)
                {
                    var lengthPluGirth = dimensions.Length + dimensions.Girth;

                    if (lengthPluGirth > Distance.FromInches(84))
                    {
                        // For parcels that measure in combined length and girth more than 84 inches but not more than 108 inches, and the piece weighs
                        // less than 20 pounds, use the 20 - pound price (balloon price) based on the applicable zone.
                        if (lengthPluGirth <= Distance.FromInches(108))
                        {
                            if (weight < Weight.FromPounds(20))
                                return _balloon.GetRate(zone);
                        }
                        else if (lengthPluGirth <= Distance.FromInches(130))
                        {
                            // For parcels that measure in combined length and girth more than 108 inches but not more than 130 inches, use the oversized price,
                            // regardless of weight, based on the applicable zone.
                            return _oversized.GetRate(zone);
                        }
                        else
                        {
                            throw new Exception(); //todo: then what?
                        }
                    }
                }

                return _base.GetRate(zone, weight);
            }
        }    
    }
}
