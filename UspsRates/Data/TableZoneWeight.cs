using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class TableZoneWeight : ITable<int, Weight>
    {
        private readonly ITableData _data;
        private readonly IIndexer<int> _columnIndex;
        private readonly IIndexer<Weight> _rowIndex;

        public TableZoneWeight(ITableData data, IIndexer<int> columnIndex, IIndexer<Weight> rowIndex)
        {
            _data = data;
            _columnIndex = columnIndex;
            _rowIndex = rowIndex;
        }

        public decimal GetRate(int zone, Weight weight)
        {
            return _data.Read(_columnIndex.GetIndex(zone), _rowIndex.GetIndex(weight));
        }
    }
}
