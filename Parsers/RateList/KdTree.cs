using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.RateList
{
    public class KdTree
    {
        public const int k = 6;

        public static void Test()
        {
            var points = new List<int[]> {
                new [] {2,3},
                new [] {5,4},
                new [] {9,6},
                new [] {4,7},
                new [] {8,1},
                new [] {7,2},
                new [] {7,3}
            };

            var points2 = new List<int[]>{
                new [] {2,3},
                new [] {2,4},
                new [] {2,5},
                new [] {2,6},
            };



            //var node = Create(Random(1495, k).Distinct(new Comp()));
            var node = Create(RandomPm(1495).Distinct(new Comp()));
            OutTop(node);

            var sw = new Stopwatch();
            sw.Start();

            var n = 1000000;
            var q = new[] { 2, 6, 1, 3, 5, 6, 7, 8, 4, 9 };
            for (var i = 0; i < n; i++)
            {
                var m = Match(node, q);
            }

            sw.Stop();
            Console.WriteLine("{0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0} ms / lookup", sw.Elapsed.TotalMilliseconds / n);
            Console.WriteLine("{0:n} lookup / sec", (n / sw.Elapsed.TotalMilliseconds) * 1000);

            Console.WriteLine(Match(node, q) != null);
        }


        public static Node Create(IEnumerable<int[]> points, int depth = 0)
        {
            if (points.Count() == 0)
                return null;

            var axis = depth % k;

            var ordered = points.OrderBy(p => p[axis]);

            //var mid = points.Sum(p => p[axis]) / points.Count(); // not working well for booleans    
            var mid = (points.Max(p => p[axis]) - points.Min(p => p[axis])) / 2D;

            var left = new List<int[]>();
            var right = new List<int[]>();

            foreach (var point in ordered)
            {
                if (point[axis] < mid)
                    left.Add(point);
                else
                    right.Add(point);
            }

            //var median = points.Count() / 2;
            //var location = ordered.ElementAt(median);

            
            var n = new Node
            {
                //Location = ordered.ElementAt(median),   // should take either the first or last element with the x at median
                //Left = Create(ordered.Take(median), depth + 1),
                //Right = Create(ordered.Skip(median + 1), depth + 1)
                Location = right.First(),
                Left = Create(left, depth + 1),
                Right = Create(right.Skip(1), depth + 1)
            };

            return n;
        }

        public static Node Match(Node node, int[] value, int depth = 0)
        {
            if (node == null || node.Location.SequenceEqual(value))
                return node;

            var axis = depth % k;

            if (value[axis] < node.Location[axis])
                return Match(node.Left, value, depth + 1);
            else
                return Match(node.Right, value, depth + 1);

        }

        public static void OutTop(Node node)
        {
            using(var writer = new StreamWriter("out.txt"))
            {
                writer.WriteLine("digraph {");

                Out(node, writer);

                writer.WriteLine("}");
            }
        }

        public static void Out(Node node, TextWriter writer)
        {
            if (node.Left != null)
            {
                writer.WriteLine("\t\"{0}\" -> \"{1}\"", node, node.Left);
                Out(node.Left, writer);
            }

            if (node.Right != null)
            {
                writer.WriteLine("\t\"{0}\" -> \"{1}\"", node, node.Right);
                Out(node.Right, writer);
            }
        }



        private static IEnumerable<int[]> Random(int count, int length = 2)
        {
            var r = new Random();

            for (var i = 0; i < count; i++)
            {
                var value = new int[length];

                for (var n = 0; n < length; n++)
                    value[n] = r.Next(15);

                yield return value;
            }
        }

        private static IEnumerable<int[]> RandomPm(int count)
        {
            var r = new Random();

            for (var i = 0; i < count; i++)
            {
                var value = new int[]
                {
                    r.Next(10), // zone
                    r.Next(70), // weight
                    r.Next(70), // dimweight
                    r.Next(2), // LplusG
                    r.Next(2), // ExceedCF
                    r.Next(2), // DimGreater
                };

                //var value = new int[]
                //{
                //    r.Next(2), // LplusG
                //    r.Next(2), // ExceedCF
                //    r.Next(2), // DimGreater
                //    r.Next(10), // zone
                //    r.Next(70), // weight
                //    r.Next(70), // dimweight
                //};

                //var value = new int[]
                //{
                //    r.Next(70), // weight
                //    r.Next(70), // dimweight
                //    r.Next(10), // zone
                //    r.Next(2), // LplusG
                //    r.Next(2), // ExceedCF
                //    r.Next(2), // DimGreater
                //};

                

                yield return value;
            }
        }

        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        private static int RandomInt(int max)
        {
            // Buffer storage.
            byte[] data = new byte[2];

            
            while(true)
            {
                // Fill buffer.
                rng.GetBytes(data);

                var v = BitConverter.ToUInt16(data, 0);

                if (v < max)
                    return v;                
            }            
        }


        private class Comp : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(int[] obj)
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = (int)2166136261;
                    
                    foreach(var o in obj)
                        hash = (hash * 16777619) ^ o.GetHashCode();

                    return hash;
                }
            }
        }

    }

    public class Node
    {
        public int[] Location;
        public Node Left;
        public Node Right;

        public override string ToString()
        {
            var t = Location.Select(l => l.ToString()).Aggregate((a, v) => String.Format("{0}, {1}", a, v));
            return String.Format("({0})", t);            
        }
    }
}
