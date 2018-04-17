using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.RateList
{
    public class Record
    {
        public int Zone = -1;
        public decimal? Weight = null;
        public decimal? DimWeight = null;        
        public bool? LPlusGGreater = null;        
        public bool? ExceedCubicFoot = null;
        public bool? DimGreater = null;
        public decimal Value;



        public Record Clone()
        {
            return (Record)MemberwiseClone();
        }

        public bool WeightEquals(decimal value)
        {
            return Weight == null || Weight == value;
        }

        public bool DimWeightEquals(decimal value)
        {
            return DimWeight == null || DimWeight == value;
        }

        public bool ZoneEquals(int value)
        {
            return Zone == null || Zone == value;
        }

        public bool DimGreaterEquals(bool value)
        {
            return DimGreater == null || DimGreater == value;
        }

        public bool ExceedCubicFootEquals(bool value)
        {
            return ExceedCubicFoot == null || ExceedCubicFoot == value;
        }

        public bool LengthGirthGreaterEquals(bool value)
        {
            return LPlusGGreater == null || LPlusGGreater == value;
        }
    }
}
