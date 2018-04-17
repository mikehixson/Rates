using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://pe.usps.com/cpim/ftp/manuals/dmm300/Notice123.pdf

namespace UspsRates
{
    public class Letter
    {
        public Weight Weight { get; }
        public bool Machinable { get; }

        public Letter(Weight weight, bool machinable = true)
        {
            Weight = weight;
            Machinable = machinable;
        }
    }


    public class Postcard
    {
        // No weight or dimensions
    }

    public class Parcel
    {
        public Weight Weight { get; private set; }
        public Dimensions Dimensions { get; private set; }

        public Parcel(Weight weight, Dimensions dimensions = null)
        {
            Weight = weight;
            Dimensions = dimensions;    //todo: dont allow null 
        }
    }


    // No weight or dimensions necessary
    public class FlatRateEnvelope
    {
 
    }

    public class LegalFlatRateEnvelope
    {
 
    }

    public class PaddedFlatRateEnvelope
    {

    }

    public class SmallFlatRateBox
    {

    }

    public class MediumFlatRateBox
    {

    }

    public class LargeFlatRateBox
    {

    }

    public class ApoFpoDpoLargeFlatRateBox
    {

    }

    public class RegionalRateBoxA
    {
        public Weight Weight { get; private set; }

        public RegionalRateBoxA(Weight weight)
        {
            Weight = weight;
        }
    }

    public class RegionalRateBoxB
    {
        public Weight Weight { get; private set; }

        public RegionalRateBoxB(Weight weight)
        {
            Weight = weight;
        }
    }


    // AKA Large Envelope
    public class Flat   // getting ready for media mail
    {
        public Weight Weight { get; private set; }
        public Dimensions Dimensions { get; set; }
        public Presort Presort { get; private set; }

        public Flat(Weight weight, Dimensions dimensions = null) //todo: remove default null
        {
            Weight = weight;
            Dimensions = dimensions;
            Presort = Presort.None;
        }

        public decimal GetRate(IZoneLookup chart, int zone)
        {
            //return chart.Get(Weight, zone);   //We need the chart to pull the info it needs from a piece if we are using generic pieces. OR MAYBE we know what type of parameters to pass based on the chart type. 
            // This could be a weight/presort chart

            return 0M;
        }
    }

    public enum Presort
    {
        None,
        Basic,
        FiveDigit
    }




/*

    



    // configure something.. give it a name, then use it to process user inputs

    public class PriorityMailExpress
    {
        private PriorityMailExpressChart _chart;

        private IZoneLookup _zoneLookup = new DummyZoneChart();

        public PriorityMailExpress(PriorityMailExpressChart chart)
        {
            _chart = chart;
        }

        // setup with mail class specific details
        public IShipment GetShipment(string origin, string destination, Parcel piece)
        {
            // why not just stick the rate in here?

            // mail class specific validations could happen here
            // this could be the place to setup everythign specific to user inputs
            // Shipment could have a general interface that every shipment would have -- GetRate, GetLabel
            // We need to make sure this doesnt become a god object

            // we could construct with these, then shipment keeps all its members private
            // As shipment is immutable, it could make sense to get these value up front at the cost of performance.
            var detail = _zoneLookup.Get(origin, destination);
            var rate = _chart.ZoneChart.Get(piece.Weight, detail.Zone);

            // we could construct with this so we dont need to lookup the rate right away
            Func<decimal> f = () => _chart.ZoneChart.Get(piece.Weight, detail.Zone);
            

            return new Shipment<Parcel>(origin, destination, piece, _chart.ZoneChart2);
        }

        // origin & destination note neessary for rating but would be useful going forward
        public IShipment GetShipment(string origin, string destination, FlatRateEnvelope piece)
        {
            return new Shipment<FlatRateEnvelope>(origin, destination, piece, _chart.FlatRateEnvelopeChart2);
        }

    }


    
    



    public class PriorityMailExpressChart
    {
        public static PriorityMailExpressChart Retail { get; private set; }
        public static PriorityMailExpressChart CommercialBase { get; private set; }

        public IZoneChart ZoneChart { get; private set; }        
        public IRateChart<Shipment<Parcel>> ZoneChart2 { get; private set; }        
        public IFlatRateChart FlatRateEnvelopeChart { get; private set; }
        public IRateChart<Shipment<FlatRateEnvelope>> FlatRateEnvelopeChart2 { get; private set; }
        public IFlatRateChart LegalFlatRateEnvelopeChart { get; private set; }
        public IFlatRateChart PaddedFlatRateEnvelopeChart { get; private set; }

        static PriorityMailExpressChart()
        {
            // These would move into an year grouping
            //Retail = new PriorityMailExpressChart(new PriorityMailExpressZoneChart(new TableData(@"C:\Users\Mike\Downloads\January 2017 - Price Files - CSV\PME Retail.dat", 8)), new FlatRateChart(23.75M), new FlatRateChart(23.95M), new FlatRateChart(24.45M));
            //CommercialBase = new PriorityMailExpressChart(new PriorityMailExpressZoneChart(new TableData(@"C:\Users\Mike\Downloads\January 2017 - Price Files - CSV\PME Comm Base.dat", 8)), new FlatRateChart(21.18M), new FlatRateChart(21.28M), new FlatRateChart(21.64M));
        }

        public PriorityMailExpressChart(IZoneChart zoneChart, IFlatRateChart flatRateEnvelopeChart, IFlatRateChart legalFlatRateEnvelopeChart, IFlatRateChart paddedFlatRateEnvelopeChart)
        {
            ZoneChart = zoneChart;
            FlatRateEnvelopeChart = flatRateEnvelopeChart;
            LegalFlatRateEnvelopeChart = legalFlatRateEnvelopeChart;
            PaddedFlatRateEnvelopeChart = paddedFlatRateEnvelopeChart;

            ZoneChart2 = (IRateChart<Shipment<Parcel>>)zoneChart;
            FlatRateEnvelopeChart2 = (IRateChart<Shipment<FlatRateEnvelope>>)flatRateEnvelopeChart;
        }
    }


 







    public interface IFlatRateChart
    {
        decimal Get();
    }

    public class FlatRateChart : IFlatRateChart, IRateChart<Shipment<FlatRateEnvelope>>
    {
        private readonly decimal _rate;

        public FlatRateChart(decimal rate)
        {
            _rate = rate;
        }

        public decimal Get()
        {
            return _rate;
        }

        public decimal GetRate(Shipment<FlatRateEnvelope> shipment)
        {
            return _rate;
        }
    }

    public interface IZoneChart
    {
        decimal Get(Weight weight, int zone);
    }



    public class PriorityMailExpressZoneChart : IZoneChart, IRateChart<Shipment<Parcel>>
    {
        private readonly ITableData _data;

        public PriorityMailExpressZoneChart(ITableData data)
        {
            _data = data;
        }

        // 10 zones
        // Local, 1, 2 in first col
        public decimal Get(Weight weight, int zone)
        {
            var w = weight.TotalPounds;

            if (w <= 0)
                throw new ApplicationException("Weight does not meet minimum.");

            if (w > 70)
                throw new ApplicationException("Weight exceeds maximum.");

            //int cols = 8;
            int y = 0;

            if (w > 0.5M)
                y = (int)Math.Ceiling(w);

            int x = 0;

            if (zone > 2)
                x = zone - 2;

            return _data.Read(x, y);
        }
        
        public decimal GetRate(Shipment<Parcel> shipment)
        {
            //var z = new DummyZoneChart();
            //var d = z.Get(shipment.Origin, shipment.Destination);

            return Get(shipment.Piece.Weight, shipment.GetZone().Zone);     // can we get shipment to pus parameters down so properties can be private?
        }
    }
*/
}
