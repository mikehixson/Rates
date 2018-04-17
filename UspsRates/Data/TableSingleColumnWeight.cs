using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class TableSingleColumnWeight : ITableSingleColumn<Weight>
    {
        private readonly ITableData _data;
        private readonly IIndexer<Weight> _rowIndex;

        public TableSingleColumnWeight(ITableData data, IIndexer<Weight> rowIndex)
        {
            _data = data;
            _rowIndex = rowIndex;
        }

        public decimal GetRate(Weight weight)
        {
            return _data.Read(0, _rowIndex.GetIndex(weight));
        }
    }
}
