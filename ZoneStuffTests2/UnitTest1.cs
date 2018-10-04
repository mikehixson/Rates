using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZoneStuffTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ZoneStuff.Class1.Zone(@"C:\Users\Mike\Downloads\charts\format2.txt");
            ZoneStuff.Class1.Exception(@"C:\Users\Mike\Downloads\2018-06-01-charts\exception.txt");

            ZoneStuff.Class1.Play();
        }

        [TestMethod]        
        public void Test2()
        {
            ZoneStuff.MyReader.Foo();
        }        
    }
}
