using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Grapher
{
    public class BMAP
    {
        public static List<int> Ids = new List<int>();

        public static int width = 512, height = width;

        public int Id { get; set; }
        public string Value { get; set; }

        public BMAP()
        { }

        public BMAP(int id, string value)
        { Id = id; Value = value; }

        public static Bitmap GetMap(int id)
        {
            switch (id)
            {
                case 0:
                    return PrimeDistribution();
            }
            return null;
        }

        public static BitmapImage GetMapImage(int id)
        { return BitmapImageFromBitmap(GetMap(id)); }

        #region conversions
        public static Bitmap BitmapFromWriteableBitmap(WriteableBitmap writewbmp)
        {
            Bitmap wbmp;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(writewbmp));
                enc.Save(outStream);
                wbmp = new Bitmap(outStream);
            }
            return wbmp;
        }

        public static BitmapImage BitmapImageFromBitmap(Bitmap iwbmp)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                Bitmap wbmp = new Bitmap(iwbmp);
                wbmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        #endregion

        //ID = 0
        public static Bitmap PrimeDistribution()
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb555);
            Random rand = new Random();
            Color b = Color.White;
            for (int by = 0; by < height; by++)
                for (int bx = 0; bx < width; bx++)
                    bmp.SetPixel(bx, by, b);
            Color c = Color.Black;
            int ll = 1;

            int x = width / 2;
            int y = height / 2;

            int i;
            int j = 1;

            for (int a = 0; a < width / 2 - 1 && a < height / 2 - 1; a++)
            {
//                Console.WriteLine(j + "/" + (width < height ? width * width : height * height));
                for (i = 0; i < ll; i++)
                {
                    if (isPrime(j))
                    { bmp.SetPixel(x + i, y, c); }
                    j++;
                }
                x += i;
                for (i = 0; i < ll; i++)
                {
                    if (isPrime(j))
                    { bmp.SetPixel(x, y - i, c); }
                    j++;
                }
                y -= i;
                ll++;
                for (i = 0; i < ll; i++)
                {
                    if (isPrime(j))
                    { bmp.SetPixel(x - i, y, c); }
                    j++;
                }
                x -= i;
                for (i = 0; i < ll; i++)
                {
                    if (isPrime(j))
                    { bmp.SetPixel(x, y + i, c); }
                    j++;
                }
                y += i;
                ll++;
            }
            bmp.SetPixel(width / 2, height / 2, Color.Red);

            Console.WriteLine(bmp.Width + ":" + bmp.Height);
            return bmp;
        }

        public static bool isPrime(BigInteger c)
        {
            if ((c & 1) == 0)
            {
                if (c == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            for (BigInteger i = 3; (i * i) <= c; i += 2)
            {
                if ((c % i) == 0)
                {
                    return false;
                }
            }
            return c != 1;
        }
    }
}
