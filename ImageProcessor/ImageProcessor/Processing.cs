using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Image = System.Drawing.Image;

namespace ImageProcessor
{
    public class Processing
    {
        private Bitmap bitmap;
        private object Locker = new object();
        private List<Bitmap> bmp { get; set; }

        /// <summary>
        ///Implements processing of one image.
        /// </summary>
        public Processing(string path)
        {
            if (path.Contains(".png"))
            {
                bitmap = (Bitmap)Image.FromFile(path);
                Bitmap hp = new Bitmap(bitmap.Width, bitmap.Height);
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        hp.SetPixel(i, j, bitmap.GetPixel(i, j));
                    }
                }
                bitmap = (Bitmap)hp.Clone();
            }
            else
                bitmap = new Bitmap(path);
        }

        /// <summary>
        /// Implements processing of several images.
        /// </summary>
        public Processing(List<Bitmap> bitmaps)
        {

            bmp = bitmaps;
        }

        #region Sequentional
        /// <summary>
        /// Changes the brightness of the image in the range of values from -100 to 100, where 0 is the brightness of the original image.
        /// </summary>
        public Bitmap Brightness(int brghtnss)
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                if (brghtnss > 255) brghtnss = 255;
                if (brghtnss < -255) brghtnss = -255;
                Color color;
                for (int i = 0; i < bmap1.Width; i++)
                {
                    for (int j = 0; j < bmap1.Height; j++)
                    {
                        color = bmap1.GetPixel(i, j);
                        int colorB = color.B + brghtnss;
                        int colorG = color.G + brghtnss;
                        int colorR = color.R + brghtnss;

                        if (colorR > 255) colorR = 255;
                        if (colorR < 0) colorR = 1;

                        if (colorG > 255) colorG = 255;
                        if (colorG < 0) colorG = 1;

                        if (colorB > 255) colorB = 255;
                        if (colorB < 0) colorB = 1;

                        bmap1.SetPixel(i, j, Color.FromArgb((byte)colorR, (byte)colorG, (byte)colorB));
                    }
                }
            }
            return bmap1;
        }

        /// <summary>
        /// Changes the contrast of the image in the range of values from -100 to 100, where 0 is the contrast of the original image.
        /// </summary>
        public Bitmap Contrast(int contrst)
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                if (contrst > 100) contrst = 100;
                if (contrst < -100) contrst = -100;
                double contrast = (100.0 + contrst) / 100.0;
                contrast *= contrast;
                Color color;
                for (int i = 0; i < bmap1.Width; i++)
                {
                    for (int j = 0; j < bmap1.Height; j++)
                    {
                        color = bmap1.GetPixel(i, j);

                        double pixelR = color.R / 255.0;
                        pixelR -= 0.5;
                        pixelR *= contrast;
                        pixelR += 0.5;
                        pixelR *= 255;
                        if (pixelR > 255) pixelR = 255;
                        if (pixelR < 0) pixelR = 0;

                        double pixelG = color.G / 255.0;
                        pixelG -= 0.5;
                        pixelG *= contrast;
                        pixelG += 0.5;
                        pixelG *= 255;
                        if (pixelG > 255) pixelG = 255;
                        if (pixelG < 0) pixelG = 0;

                        double pixelB = color.B / 255.0;
                        pixelB -= 0.5;
                        pixelB *= contrast;
                        pixelB += 0.5;
                        pixelB *= 255;
                        if (pixelB < 0) pixelB = 0;
                        if (pixelB > 255) pixelB = 255;

                        bmap1.SetPixel(i, j, Color.FromArgb((byte)pixelR, (byte)pixelG, (byte)pixelB));
                    }
                }
            }
            return bmap1;
        }

        /// <summary>
        /// Applies a grayscale filter to the image.
        /// </summary>
        public Bitmap Grayscale()
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                Color color;
                for (int i = 0; i < bmap1.Width; i++)
                {
                    for (int j = 0; j < bmap1.Height; j++)
                    {
                        color = bmap1.GetPixel(i, j);
                        byte g = (byte)(.299 * color.R + .587 * color.G + .114 * color.B);
                        bmap1.SetPixel(i, j, Color.FromArgb(g, g, g));
                    }
                }
            }
            return bmap1;
        }

        /// <summary>
        /// Inverts the colors of the image.
        /// </summary>
        public Bitmap Invert()
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                Color color;
                for (int i = 0; i < bmap1.Width; i++)
                {
                    for (int j = 0; j < bmap1.Height; j++)
                    {
                        color = bmap1.GetPixel(i, j);
                        bmap1.SetPixel(i, j, Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B));
                    }
                }
            }
            return bmap1;
        }

        /// <summary>
        /// Adds a sepia tone to the image.
        /// </summary>
        public Bitmap Sepia()
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                Color color;

                for (int y = 0; y < bmap1.Height; y++)
                {
                    for (int x = 0; x < bmap1.Width; x++)
                    {
                        color = bmap1.GetPixel(x, y);

                        int pA = color.A;
                        int pR = color.R;
                        int pG = color.G;
                        int pB = color.B;

                        int r = (int)(0.393 * pR + 0.769 * pG + 0.189 * pB);
                        int g = (int)(0.349 * pR + 0.686 * pG + 0.168 * pB);
                        int b = (int)(0.272 * pR + 0.534 * pG + 0.131 * pB);

                        if (r > 255) pR = 255;
                        else pR = r;

                        if (g > 255) pG = 255;
                        else pG = g;

                        if (b > 255) pB = 255;
                        else pB = b;

                        bmap1.SetPixel(x, y, Color.FromArgb(pA, pR, pG, pB));
                    }
                }
            }
            return bmap1;
        }

        /// <summary>
        /// Rotates the image by 90, 180 and 270 degrees. Accepts as input the numerical value of the turn from the ones offered above.
        /// </summary>
        public Bitmap Rotate(int degree)
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                if (degree == 90) bmap1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                if (degree == 180) bmap1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                if (degree == 270) bmap1.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            return bmap1;
        }

        /// <summary>
        /// Flipping the image horizontally or vertically, where 0 is displayed horizontally, and 1 is displayed vertically.
        /// </summary>
        public Bitmap Flip(int rotate)
        {
            Bitmap bmap1;

            lock (Locker)
            {
                bmap1 = (Bitmap)bitmap.Clone();

                if (rotate == 0) bmap1.RotateFlip(RotateFlipType.RotateNoneFlipX);
                if (rotate == 1) bmap1.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            return bmap1;
        }
        #endregion

        #region Pool

        /// <summary>
        /// Changes the brightness of each image from the list in the range of values from -100 to 100, where 0 is the brightness of the original image.
        /// </summary>
        public List<Bitmap> ParallelBrightness(int brghtnss)
        {
            List<Bitmap> result = new List<Bitmap>();
            using (CountdownEvent cuuntdeve = new CountdownEvent(1))
            {
                ManualResetEvent manualResEve = new ManualResetEvent(false);

                foreach (var v in bmp)
                {
                    bool isEnqueued = ThreadPool.QueueUserWorkItem(delegate
                    {
                        cuuntdeve.AddCount();
                        bitmap = v;
                        result.Add(Brightness(brghtnss));
                        cuuntdeve.Signal();
                        manualResEve.Set();
                    });
                }
                manualResEve.WaitOne();
                cuuntdeve.Signal();
                cuuntdeve.Wait();
            }
            return result;
        }

        /// <summary>
        /// Changes the contrast of each image from the list in the range of values from -100 to 100, where 0 is the contrast of the original image.
        /// </summary>
        public List<Bitmap> ParallelContrast(int contrst)
        {
            List<Bitmap> result = new List<Bitmap>();
            using (CountdownEvent cuuntdeve = new CountdownEvent(1))
            {
                ManualResetEvent manualResEve = new ManualResetEvent(false);

                foreach (var v in bmp)
                {
                    bool isEnqueued = ThreadPool.QueueUserWorkItem(delegate
                    {
                        cuuntdeve.AddCount();
                        bitmap = v;
                        result.Add(Contrast(contrst));
                        cuuntdeve.Signal();
                        manualResEve.Set();
                    });
                }
                manualResEve.WaitOne();
                cuuntdeve.Signal();
                cuuntdeve.Wait();
            }
            return result;
        }

        /// <summary>
        /// Applies a grayscale filter to each image in the list.
        /// </summary>
        public List<Bitmap> ParallelGrayscale()
        {
            List<Bitmap> result = new List<Bitmap>();
            using (CountdownEvent cuuntdeve = new CountdownEvent(1))
            {
                ManualResetEvent manualResEve = new ManualResetEvent(false);

                foreach (var v in bmp)
                {
                    bool isEnqueued = ThreadPool.QueueUserWorkItem(delegate
                    {
                        cuuntdeve.AddCount();
                        bitmap = v;
                        result.Add(Grayscale());
                        cuuntdeve.Signal();
                        manualResEve.Set();
                    });
                }
                manualResEve.WaitOne();
                cuuntdeve.Signal();
                cuuntdeve.Wait();
            }
            return result;
        }

        /// <summary>
        /// Inverts the colors of each image in the list.
        /// </summary>
        public List<Bitmap> ParallelInvert()
        {
            List<Bitmap> result = new List<Bitmap>();
            using (CountdownEvent cuuntdeve = new CountdownEvent(1))
            {
                ManualResetEvent manualResEve = new ManualResetEvent(false);

                foreach (var v in bmp)
                {
                    bool isEnqueued = ThreadPool.QueueUserWorkItem(delegate
                    {
                        cuuntdeve.AddCount();
                        bitmap = v;
                        result.Add(Invert());
                        cuuntdeve.Signal();
                        manualResEve.Set();
                    });
                }
                manualResEve.WaitOne();
                cuuntdeve.Signal();
                cuuntdeve.Wait();
            }
            return result;
        }

        /// <summary>
        /// Adds a sepia tone to each image in the list.
        /// </summary>
        public List<Bitmap> ParallelSepia()
        {
            List<Bitmap> result = new List<Bitmap>();
            using (CountdownEvent cuuntdeve = new CountdownEvent(1))
            {
                ManualResetEvent manualResEve = new ManualResetEvent(false);

                foreach (var v in bmp)
                {
                    bool isEnqueued = ThreadPool.QueueUserWorkItem(delegate
                    {
                        cuuntdeve.AddCount();
                        bitmap = v;
                        result.Add(Sepia());
                        cuuntdeve.Signal();
                        manualResEve.Set();
                    });
                }
                manualResEve.WaitOne();
                cuuntdeve.Signal();
                cuuntdeve.Wait();
            }
            return result;
        }

        /// <summary>
        /// Rotates each image in the list by 90, 180, and 270 degrees. Accepts as input the numerical value of the turn from the ones offered above.
        /// </summary>
        public List<Bitmap> ParallelRotate(int degree)
        {
            List<Bitmap> result = new List<Bitmap>();
            foreach (var v in bmp)
                result.Add(Rotate(degree));
            return result;
        }

        /// <summary>
        /// Flipping each image from the list horizontally or vertically, where 0 is displayed horizontally and 1 is displayed vertically.
        /// </summary>
        public List<Bitmap> ParallelFlip(int rotate)
        {
            List<Bitmap> result = new List<Bitmap>();
            foreach (var v in bmp)
                result.Add(Flip(rotate));
            return result;
        }
        #endregion
    }
}
