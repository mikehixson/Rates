using System;
using System.Threading.Tasks;
using Xunit;
using ZoneStuff;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace ZoneStuffTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            ZoneStuff.Class1.Zone(@"C:\Users\Mike\Downloads\charts\format2.txt");
            ZoneStuff.Class1.Exception(@"C:\Users\Mike\Downloads\2018-06-01-charts\exception.txt");

            ZoneStuff.Class1.Play();
        }

        [Fact]
        public async Task Test2()
        {
            await MyReader.Pushed();
        }

        [Fact]
        public async Task Test3()
        {
            var p = new ExceptionFileAnalyzer();
            await p.Foo();
        }

        [Fact]
        public void FileObserverTest()
        {


            

        

            //o.Wait();
        }
    }
}
