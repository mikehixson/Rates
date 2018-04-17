using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates.Data
{
    public class TableSingleCell : ITableSingleCell
    {
        private readonly ITableData _data;

        public TableSingleCell(ITableData data)
        {
            _data = data;
        }

        public decimal GetRate()
        {
            return _data.Read(0, 0);
        }
    }
}
