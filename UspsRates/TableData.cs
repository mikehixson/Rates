using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UspsRates
{
    public interface ITableData
    {
        short Columns { get; }
        short Rows { get; }
        decimal Read(int x, int y);
    }

    public class TableData : ITableData
    {
        private int[] _values;
        public short Columns { get; private set; }
        public short Rows { get; private set; }

        public TableData()
        {

        }

        public TableData(string filePath)
        {
            _values = Parse(filePath).ToArray();
        }

        public decimal Read(int x, int y)
        {
            if (!(x < Columns))
                throw new ArgumentOutOfRangeException(nameof(x));

            if (!(y < Rows))
                throw new ArgumentOutOfRangeException(nameof(y));

            int index = (y * Columns) + x;

            return _values[index] / 1000M;
        }

        private IEnumerable<int> Parse(string filePath)
        {
            var values = new List<int>();

            using (var stream = File.OpenRead(filePath))
            using (var reader = new BinaryReader(stream))
            {
                return Parse(reader).ToArray();
            }
        }

        private IEnumerable<int> Parse(BinaryReader reader)
        {
            Columns = reader.ReadInt16();
            Rows = reader.ReadInt16();
            
            var end = reader.BaseStream.Position + (Columns * Rows * sizeof(Int32));

            while (reader.BaseStream.Position < end)
                yield return reader.ReadInt32();    //todo: initialize array instead

        }

        public void Load(BinaryReader reader)
        {
            _values = Parse(reader).ToArray();
        }
    }

    public interface ITableSet
    {
        ITableData this[int i] { get; }
    }


    public class TableSet : IEnumerable<TableInfo>  // should be ok to tkae path in the constructor... as long as this is abstracted to an interface.
    {
        private TableInfo[] _tables;

        public TableInfo this[int i]
        {
            get { return _tables[i]; }
        }

        public TableSet()
        {
            _tables = new TableInfo[0];
        }

        public void Load(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var reader = new BinaryReader(stream))
            {
                Load(reader);
            }
        }

        public void Load(BinaryReader reader)       //todo: no sure if i like the binaryReader here stream is better?
        {
            _tables = Parse(reader).ToArray();
        }

        public IEnumerator<TableInfo> GetEnumerator()
        {
            return ((IEnumerable<TableInfo>)_tables).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tables.GetEnumerator();
        }

        private IEnumerable<TableInfo> Parse(BinaryReader reader)
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var info = new TableInfo();
                info.Table = reader.ReadString();    //todo: prefer to do null terminated string?
                info.ColumnIndexer = reader.ReadString();
                info.RowIndexer = reader.ReadString();

                if (info.ColumnIndexer == String.Empty)
                    info.ColumnIndexer = null;

                if (info.RowIndexer == String.Empty)
                    info.RowIndexer = null;

                var table = new TableData();
                table.Load(reader);

                info.Data = table;

                yield return info;
            }
        }

        public static TableSet Create(string filePath)
        {
            TableSet set = new TableSet();
            set.Load(filePath);

            return set;
        }
    }

    public class TableInfo
    {
        public string Table { get; set; }
        public string ColumnIndexer { get; set; }
        public string RowIndexer { get; set; }
        public ITableData Data { get; set; }
    }
}
