using RTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Collections;
using Konscious.SpatialIndex;
using UltimateQuadTree;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Draw.DoIt("medium.png", GetEm2());
            Draw.DoIt("largest.png", GetEm());
            //Spawwwtial();
            Em();
            //RTree();
        }

        static void Quad2()
        {



        }

        static void Quad1()
        {





        }

        private class Bounds : IQuadTreeObjectBounds<(int X1, int X2, int Y1, int Y2)>
        {
            public double GetBottom((int X1, int X2, int Y1, int Y2) obj)
            {
                return obj.Y1;
            }

            public double GetLeft((int X1, int X2, int Y1, int Y2) obj)
            {
                return obj.X1;
            }

            public double GetRight((int X1, int X2, int Y1, int Y2) obj)
            {
                return obj.X2;
            }

            public double GetTop((int X1, int X2, int Y1, int Y2) obj)
            {
                return obj.Y2;
            }
        }

        static void Spawwwtial()
        {
            var options = new RTreeOptions()
            {
                Dimensions = 2,
                EnsureTightMBRs = true,
                TreeVariant = RTreeVariant.RStar,
                FillFactor = 0.9
            };

            var tree = new Konscious.SpatialIndex.RTree<int>(options, new MemoryStorageManager());

            var i = 0;
            foreach (var rect in GetEm())
                tree.Add(new Region(new double[] { rect.X1, rect.Y1 }, new double[] { rect.X2, rect.Y2 }), i++);

            var sw = new Stopwatch();
            sw.Start();

            var r = tree.IntersectsWith(new Region(new double[] { 09749, 86576 }, new double[] { 09749, 86576 }));
            Console.WriteLine(sw.ElapsedTicks);
        }

        static void Em()
        {
            var tree = new Enyim.Collections.RTree<int>();

            var items = GetEm().Select((rect, i) => new RTreeNode<int>(i, new Envelope(rect.X1, rect.Y1, rect.X2, rect.Y2))).ToList();
            tree.Load(items);

            //foreach (var rect in GetEm())
            //    tree.Insert(new RTreeNode<int>(i++, new Envelope(rect.X1, rect.Y1, rect.X2, rect.Y2)));

            //tree.Insert(new RTreeNode<int>(0, new Envelope(0, 0, 10, 10)));

            var sw = new Stopwatch();
            sw.Start();

            var r = tree.Search(new Envelope(09749, 86576, 09749, 86576));
            Console.WriteLine(sw.ElapsedTicks);
        }

        static void RTree()
        {
            var tree = new RTree.RTree<string>();
            tree.Add(new Rectangle(0, 0, 10, 10, 0, 0), "ten");

            tree.Add(new Rectangle(0, 20, 40, 40, 0, 0), "twwenth");


            var sw = new Stopwatch();
            sw.Start();

            var r = tree.Intersects(new Rectangle(0, 0, 0, 0, 0, 0));
            Console.WriteLine(sw.ElapsedTicks);
        }

        //097 - 865
        private static IEnumerable<(int X1, int X2, int Y1, int Y2, int Z)> GetEm2()
        {
            yield return (09761 - 09700, 09761 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09780 - 09700, 09780 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09706 - 09700, 09706 - 09700, 86500 - 86500, 86599 - 86500, 7);
            yield return (09714 - 09700, 09714 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09720 - 09700, 09720 - 09700, 86500 - 86500, 86599 - 86500, 7);
            yield return (09722 - 09700, 09722 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09725 - 09700, 09725 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09729 - 09700, 09729 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09732 - 09700, 09732 - 09700, 86500 - 86500, 86599 - 86500, 7);
            yield return (09735 - 09700, 09735 - 09700, 86500 - 86500, 86599 - 86500, 7);
            yield return (09743 - 09700, 09743 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09745 - 09700, 09745 - 09700, 86500 - 86500, 86599 - 86500, 5);
            yield return (09749 - 09700, 09749 - 09700, 86500 - 86500, 86599 - 86500, 5);
            yield return (09754 - 09700, 09756 - 09700, 86500 - 86500, 86599 - 86500, 6);
            yield return (09760 - 09700, 09760 - 09700, 86500 - 86500, 86599 - 86500, 5);
        }

        //097 - 969 (Biggest Set)
        private static IEnumerable<(int X1, int X2, int Y1, int Y2, int Z)> GetEm()
        {
            yield return (09714-09700, 09714-09700, 96900-96900, 96999-96900, 8);
            yield return (09722-09700, 09722-09700, 96900-96900, 96999-96900, 8);
            yield return (09725-09700, 09725-09700, 96900-96900, 96999-96900, 8);
            yield return (09729-09700, 09729-09700, 96900-96900, 96999-96900, 8);
            yield return (09743-09700, 09743-09700, 96900-96900, 96999-96900, 8);
            yield return (09745-09700, 09745-09700, 96900-96900, 96999-96900, 8);
            yield return (09749-09700, 09749-09700, 96900-96900, 96999-96900, 8);
            yield return (09754-09700, 09756-09700, 96900-96900, 96999-96900, 8);
            yield return (09760-09700, 09760-09700, 96900-96900, 96999-96900, 1);
            yield return (09761-09700, 09761-09700, 96900-96900, 96999-96900, 8);
            yield return (09780-09700, 09780-09700, 96900-96900, 96999-96900, 8);
            yield return (09706-09700, 09706-09700, 96900-96900, 96999-96900, 8);
            yield return (09720-09700, 09720-09700, 96900-96900, 96999-96900, 8);
            yield return (09732-09700, 09732-09700, 96900-96900, 96999-96900, 8);
            yield return (09735-09700, 09735-09700, 96900-96900, 96999-96900, 8);
            yield return (09700-09700, 09705-09700, 96900-96900, 96938-96900, 8);
            yield return (09707-09700, 09713-09700, 96900-96900, 96938-96900, 8);
            yield return (09715-09700, 09719-09700, 96900-96900, 96938-96900, 8);
            yield return (09721-09700, 09721-09700, 96900-96900, 96938-96900, 8);
            yield return (09723-09700, 09724-09700, 96900-96900, 96938-96900, 8);
            yield return (09726-09700, 09728-09700, 96900-96900, 96938-96900, 8);
            yield return (09730-09700, 09731-09700, 96900-96900, 96938-96900, 8);
            yield return (09733-09700, 09734-09700, 96900-96900, 96938-96900, 8);
            yield return (09736-09700, 09742-09700, 96900-96900, 96938-96900, 8);
            yield return (09744-09700, 09753-09700, 96900-96900, 96938-96900, 8);
            yield return (09757-09700, 09760-09700, 96900-96900, 96938-96900, 8);
            yield return (09762-09700, 09779-09700, 96900-96900, 96938-96900, 8);
            yield return (09781-09700, 09799-09700, 96900-96900, 96938-96900, 8);
            yield return (09700-09700, 09705-09700, 96945-96900, 96959-96900, 8);
            yield return (09707-09700, 09713-09700, 96945-96900, 96959-96900, 8);
            yield return (09715-09700, 09719-09700, 96945-96900, 96959-96900, 8);
            yield return (09721-09700, 09721-09700, 96945-96900, 96959-96900, 8);
            yield return (09723-09700, 09724-09700, 96945-96900, 96959-96900, 8);
            yield return (09726-09700, 09728-09700, 96945-96900, 96959-96900, 8);
            yield return (09730-09700, 09731-09700, 96945-96900, 96959-96900, 8);
            yield return (09733-09700, 09734-09700, 96945-96900, 96959-96900, 8);
            yield return (09736-09700, 09742-09700, 96945-96900, 96959-96900, 8);
            yield return (09744-09700, 09753-09700, 96945-96900, 96959-96900, 8);
            yield return (09757-09700, 09760-09700, 96945-96900, 96959-96900, 8);
            yield return (09762-09700, 09779-09700, 96945-96900, 96959-96900, 8);
            yield return (09781-09700, 09799-09700, 96945-96900, 96959-96900, 8);
            yield return (09700-09700, 09705-09700, 96961-96900, 96969-96900, 8);
            yield return (09707-09700, 09713-09700, 96961-96900, 96969-96900, 8);
            yield return (09715-09700, 09719-09700, 96961-96900, 96969-96900, 8);
            yield return (09721-09700, 09721-09700, 96961-96900, 96969-96900, 8);
            yield return (09723-09700, 09724-09700, 96961-96900, 96969-96900, 8);
            yield return (09726-09700, 09728-09700, 96961-96900, 96969-96900, 8);
            yield return (09730-09700, 09731-09700, 96961-96900, 96969-96900, 8);
            yield return (09733-09700, 09734-09700, 96961-96900, 96969-96900, 8);
            yield return (09736-09700, 09742-09700, 96961-96900, 96969-96900, 8);
            yield return (09744-09700, 09753-09700, 96961-96900, 96969-96900, 8);
            yield return (09757-09700, 09760-09700, 96961-96900, 96969-96900, 8);
            yield return (09762-09700, 09779-09700, 96961-96900, 96969-96900, 8);
            yield return (09781-09700, 09799-09700, 96961-96900, 96969-96900, 8);
            yield return (09700-09700, 09705-09700, 96971-96900, 96999-96900, 8);
            yield return (09707-09700, 09713-09700, 96971-96900, 96999-96900, 8);
            yield return (09715-09700, 09719-09700, 96971-96900, 96999-96900, 8);
            yield return (09721-09700, 09721-09700, 96971-96900, 96999-96900, 8);
            yield return (09723-09700, 09724-09700, 96971-96900, 96999-96900, 8);
            yield return (09726-09700, 09728-09700, 96971-96900, 96999-96900, 8);
            yield return (09730-09700, 09731-09700, 96971-96900, 96999-96900, 8);
            yield return (09733-09700, 09734-09700, 96971-96900, 96999-96900, 8);
            yield return (09736-09700, 09742-09700, 96971-96900, 96999-96900, 8);
            yield return (09744-09700, 09753-09700, 96971-96900, 96999-96900, 8);
            yield return (09757-09700, 09760-09700, 96971-96900, 96999-96900, 8);
            yield return (09762-09700, 09779-09700, 96971-96900, 96999-96900, 8);
            yield return (09781-09700, 09799-09700, 96971-96900, 96999-96900, 8);
        }
    }
}

