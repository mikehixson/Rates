using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public static class Draw
    {
        public static void DoIt(string name, IEnumerable<(int X1, int X2, int Y1, int Y2, int Z)> rectangles)
        {
            using (Image<Rgba32> image = new Image<Rgba32>(Configuration.Default, 1020, 1020, Rgba32.WhiteSmoke))
            {

                image.Mutate(x => x.DrawLines());

                var pen = Pens.Solid<Rgba32>(Rgba32.Black, 1);
                var pink = Rgba32.HotPink;
                var green = Rgba32.Green;
                var purple = Rgba32.Purple;

                foreach (var r in rectangles)
                {
                    Console.WriteLine($"{r.X1},{r.Y1} {r.X2},{r.Y2}");

                    (int X1, int X2, int Y1, int Y2, int Z) j = (r.X1 * 10, r.X2 * 10, r.Y1 * 10, r.Y2 * 10, r.Z);

                    

                    Rgba32 color;

                    //if (j.X2 - j.X1 == 0)
                    //    color = pink;
                    //else if (j.Y2 - j.Y1 == 0)
                    //    color = purple;
                    //else
                    //{
                    //    if(j.X2 - j.X1 <= j.Y2 - j.Y1)
                    //        color = green;
                    //    else
                    //        color= Rgba32.Orange;
                    //}

                    switch(j.Z)
                    {
                        case 1: color = Rgba32.Purple; break;
                        case 2: color = Rgba32.Blue; break;
                        case 3: color = Rgba32.LightGreen; break;
                        case 4: color = Rgba32.Green; break;
                        case 5: color = Rgba32.Yellow; break;
                        case 6: color = Rgba32.Orange; break;
                        case 7: color = Rgba32.Purple; break;
                        case 8: color = Rgba32.Red; break;
                        default: color = Rgba32.Black; break;

                    }

                    color.A = 100;

                    image.Mutate(x => x.Fill(color, new Rectangle(j.X1, j.Y1, (j.X2 - j.X1) + 10, (j.Y2 - j.Y1) + 10)));
                    image.Mutate(x => x.Draw(pen, new RectangleF(j.X1, j.Y1, (j.X2 - j.X1) + 10, (j.Y2 - j.Y1) + 10)));
                }

                image.SaveAsPng(File.OpenWrite(name));
            }
        }
    }
}
