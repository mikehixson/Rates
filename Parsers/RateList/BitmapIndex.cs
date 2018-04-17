using Ewah;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.RateList
{
    class BitmapIndex
    {
        private static Random _random = new Random();

        private const int ZoneIndex = 0;
        private const int ZoneLength = 10;
        private const int WeightIndex = 10;
        private const int WeightLength = 70;


        public static void Foo()
        {
            Ewah();
            BitArray();
            Advanced();
            Simple();
        }

        public static void Ewah()
        {
            var a = Setup(128);
            var b = Setup(128);

            var sw = new Stopwatch();
            sw.Start();

            // Zone 5 & Weight 20
            var j = 1000000;
            for (var i = 0; i < j; i++)
            {
                var v = a.And(b);

                /*
                v.CopyTo(t);
                BitArray b = new BitArray(t);

                List<decimal> r = new List<decimal>();

                for (int k = 0; k < 128; k++)
                {
                    if (b[k])
                        r.Add(data[k]);
                }
                */
            }

            sw.Stop();
            Console.WriteLine("--- Ewah ---");
            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / j);
            Console.WriteLine("{0:n} lookup / sec", (j / sw.Elapsed.TotalMilliseconds) * 1000);
            Console.WriteLine();
        }

        private static void BitArray()
        {
            var a = new BitArray(GetRandomBytes(128));
            var b = new BitArray(GetRandomBytes(128));

            var sw = new Stopwatch();
            sw.Start();

            // Zone 5 & Weight 20
            var j = 1000000;
            for (var i = 0; i < j; i++)
            {
                var v = a.And(b);
                
                /*
                v.CopyTo(t);
                BitArray b = new BitArray(t);

                List<decimal> r = new List<decimal>();

                for (int k = 0; k < 128; k++)
                {
                    if (b[k])
                        r.Add(data[k]);
                }
                */
            }

            sw.Stop();
            Console.WriteLine("--- BitArray ---");
            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / j);
            Console.WriteLine("{0:n} lookup / sec", (j / sw.Elapsed.TotalMilliseconds) * 1000);
            Console.WriteLine();
        }


        private static void Advanced()
        {
            var a = new MyVector(GetRandomBytes(128));
            var b = new MyVector(GetRandomBytes(128));

            var sw = new Stopwatch();
            sw.Start();

            // Zone 5 & Weight 20
            var j = 1000000;
            for (var i = 0; i < j; i++)
            {
                var v = a.And(b);

                /*
                v.CopyTo(t);
                BitArray b = new BitArray(t);

                List<decimal> r = new List<decimal>();

                for (int k = 0; k < 128; k++)
                {
                    if (b[k])
                        r.Add(data[k]);
                }
                */
            }

            sw.Stop();
            Console.WriteLine("--- Advanced ---");
            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / j);
            Console.WriteLine("{0:n} lookup / sec", (j / sw.Elapsed.TotalMilliseconds) * 1000);
            Console.WriteLine();
        }

        private static void Simple()
        {
            var a = new Vector<byte>(GetRandomBytes(16));
            var b = new Vector<byte>(GetRandomBytes(16));

            var sw = new Stopwatch();
            sw.Start();

            // Zone 5 & Weight 20
            var j = 1000000;
            for (var i = 0; i < j; i++)
            {
                for (var n = 0; n < 8; n++)
                {
                    var v = a & b;
                }

                /*
                v.CopyTo(t);
                BitArray b = new BitArray(t);

                List<decimal> r = new List<decimal>();

                for (int k = 0; k < 128; k++)
                {
                    if (b[k])
                        r.Add(data[k]);
                }
                */
            }

            sw.Stop();
            Console.WriteLine("--- Simple ---");
            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / j);
            Console.WriteLine("{0:n} lookup / sec", (j / sw.Elapsed.TotalMilliseconds) * 1000);
            Console.WriteLine();
        }


        private static ulong GetRandom()
        {
            var buffer = new byte[8];

            _random.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        private static byte[] GetRandomBytes(int length)
        {
            var buffer = new byte[length];

            _random.NextBytes(buffer);

            return buffer;
        }


        private static EwahCompressedBitArray Setup(int bits)
        {
            var a = new EwahCompressedBitArray();

            for (int i = 0; i < bits; i++)
            {
                if (_random.NextDouble() < 0.5)
                    a.Set(i);
            }

            return a;
        }
    }

    public class MyVector
    {
        private List<Vector<byte>> _list;

        public MyVector(byte[] bytes)
        {
            _list = new List<Vector<byte>>();
            
            for (int i = 0; i < bytes.Length; i += 16)
                _list.Add(new Vector<byte>(bytes, i));
        }

        private MyVector(List<Vector<byte>> list)
        {
            _list = list;
        }

        public MyVector And(MyVector vector)
        {
            var a = _list;
            var b = vector._list;

            //var l = new List<Vector<byte>>(a.Count);

            for (var i = 0; i < _list.Count; i++)
            {
                //l.Add(a[i] & b[i]);
                var c = a[i] & b[i];
            }

            //return new MyVector(l);
            return null;
        }

        public MyVector And(MyVector vector1, MyVector vector2, MyVector vector3)
        {
            var a = _list;
            var b = vector1._list;
            var c = vector2._list;
            var d = vector3._list;

            var l = new List<Vector<byte>>();

            for (var i = 0; i < _list.Count; i++)
                l.Add(a[i] & b[i] & c[i] & d[i]);

            return new MyVector(l);
        }


        public MyVector And2(MyVector vector1, MyVector vector2, MyVector vector3)
        {
            var a = _list;
            var b = vector1._list;
            var c = vector2._list;
            var d = vector3._list;

            var l = new Vector<byte>[a.Count];

            for (var i = 0; i < _list.Count; i++)
                l[i] = (a[i] & b[i] & c[i] & d[i]);

            //Parallel.For(0, _list.Count, i =>
            //{
            //    l[i] = (a[i] & b[i] & c[i] & d[i]);
            //});

            return new MyVector(l.ToList());
        }
    }
}
