using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UspsRates.Data;
using Xunit;

namespace UspsRatesTest
{
    public class IndexerVolumeCuFtPoint1ToPoint5Tests
    {
        [Fact]
        public void GetIndex_ValueTooBig_ThrowsException()
        {
            var index = new IndexerVolumeCuFtPoint1ToPoint5();

            Assert.Throws<ArgumentOutOfRangeException>(() => index.GetIndex(0.51M));
        }

        [Fact]
        public void GetIndex_ValueValid_GetsCorrectIndex()
        {
            var index = new IndexerVolumeCuFtPoint1ToPoint5();

            var value = index.GetIndex(0.21M);

            Assert.Equal(2, value);
        }
    }
}
