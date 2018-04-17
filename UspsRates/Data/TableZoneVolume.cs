using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class TableZoneVolume : ITable<int, decimal>
    {
        private readonly ITableData _data;
        private readonly IIndexer<int> _columnIndex;
        private readonly IIndexer<decimal> _rowIndex;

        public TableZoneVolume(ITableData data, IIndexer<int> columnIndex, IIndexer<decimal> rowIndex)
        {
            _data = data;
            _columnIndex = columnIndex;
            _rowIndex = rowIndex;
        }

        public decimal GetRate(int zone, decimal volume)
        {
            return _data.Read(_columnIndex.GetIndex(zone), _rowIndex.GetIndex(volume));
        }
    }
}
