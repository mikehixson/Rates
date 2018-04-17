using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class TableSingleRowZone : ITableSingleRow<int>       // todo: this could just be "Single Row Table"? - TableZoneSingleRow
    {
        private readonly ITableData _data;
        private readonly IIndexer<int> _columnIndex;

        public TableSingleRowZone(ITableData data, IIndexer<int> columnIndex)
        {
            _data = data;
            _columnIndex = columnIndex;
        }

        public decimal GetRate(int zone)
        {
            return _data.Read(_columnIndex.GetIndex(zone), 0);
        }
    }
}
