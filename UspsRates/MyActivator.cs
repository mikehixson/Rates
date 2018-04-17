using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public class MyActivator
    {
        public static T Get<T>(TableSet set)
        {
            var parameters = new List<object>();

            foreach(var t in set)
                parameters.Add(CreateTable(t.Table, t.ColumnIndexer, t.RowIndexer, t.Data));

            return (T)Activator.CreateInstance(typeof(T), parameters.ToArray());
        }

        private static object CreateTable(string table, string col, string row, ITableData tableData)
        {
            var tableType = Type.GetType($"UspsRates.Data.Table{table}");

            var parameters = new List<object>();

            parameters.Add(tableData); //todo:

            if (col != null)
            {
                var colType = Type.GetType($"UspsRates.Data.Indexer{col}");
                var colInstance = Activator.CreateInstance(colType);
                parameters.Add(colInstance);
            }

            if (row != null)
            {
                var rowType = Type.GetType($"UspsRates.Data.Indexer{row}");
                var rowInstance = Activator.CreateInstance(rowType);
                parameters.Add(rowInstance);
            }

            return Activator.CreateInstance(tableType, parameters.ToArray());

               
        }
    }
}
