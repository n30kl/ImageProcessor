using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Security.Principal;
using System.Threading;
using Image = System.Drawing.Image;

string imagePath = @"C:\Тестування\img.png";
string savePath = @"C:\Тестування\Тестування1\";

while (true)
{
    #region ---Тест послідовного алгоритму---
    /*Library filters = new Library(imagePath);

    filters.Brightness(50).Save($"{savePath}Brightness.png");
    filters.Contrast(4).Save($"{savePath}Contrast.png");
    filters.Grayscale().Save($"{savePath}Grayscale.png");
    filters.Invert().Save($"{savePath}Invert.png");
    filters.Sepia().Save($"{savePath}Sepia.png");

    filters.Rotate(90).Save($"{savePath}Rotate90.png");
    filters.Rotate(180).Save($"{savePath}Rotate180.png");
    filters.Rotate(270).Save($"{savePath}Rotate270.png");
    filters.Flip("horizontal").Save($"{savePath}horizontal.png");
    filters.Flip("vertical").Save($"{savePath}vertical.png");
    
    Console.ReadLine();*/
    #endregion

    #region ---Тест Parallel.Foreach алгоритму---

    /*Bitmap image = new Bitmap(imagePath);
    List<Bitmap> bitmaps = new List<Bitmap>();
    Console.Write("count = ");
    int count = Convert.ToInt32(Console.ReadLine());
    for (int i = 0; i < count; i++) bitmaps.Add(image);
    
    Library filters = new Library();
    filters.bmp = bitmaps;

    SaveBitmaps(filters.PBrightness(50), "Brightness");
    SaveBitmaps(filters.PContrast(4), "Contrast");
    SaveBitmaps(filters.PGrayscale(), "Grayscale");
    SaveBitmaps(filters.PInvert(), "Invert");
    SaveBitmaps(filters.PSepia(), "Sepia");

    SaveBitmaps(filters.PRotate(90), "Rotate90_");
    SaveBitmaps(filters.PRotate(180), "Rotate180_");
    SaveBitmaps(filters.PRotate(270), "Rotate270_");
    SaveBitmaps(filters.PFlip("horizontal"), "horizontal");
    SaveBitmaps(filters.PFlip("vertical"), "vertical");*/
    #endregion

    #region ---Тест Thread алгоритму---

    /*Bitmap image = new Bitmap(imagePath);
    List<Bitmap> bitmaps = new List<Bitmap>();
    Console.Write("count = ");
    int count = Convert.ToInt32(Console.ReadLine());
    for (int i = 0; i < count; i++) bitmaps.Add(image);
    
    Library filters = new Library();
    filters.bmp = bitmaps;

    SaveBitmaps(filters.TBrightness(50), "Brightness");
    SaveBitmaps(filters.TContrast(4), "Contrast");
    SaveBitmaps(filters.TGrayscale(), "Grayscale");
    SaveBitmaps(filters.TInvert(), "Invert");
    SaveBitmaps(filters.TSepia(), "Sepia");

    SaveBitmaps(filters.TRotate(90), "Rotate90_");
    SaveBitmaps(filters.TRotate(180), "Rotate180_");
    SaveBitmaps(filters.TRotate(270), "Rotate270_");
    SaveBitmaps(filters.TFlip("horizontal"), "horizontal");
    SaveBitmaps(filters.TFlip("vertical"), "vertical");*/
    #endregion

    #region ---Тест ThreadPool алгоритму---

    Bitmap image = new Bitmap(imagePath);
    List<Bitmap> bitmaps = new List<Bitmap>();
    Console.Write("count = ");
    int count = Convert.ToInt32(Console.ReadLine());
    for (int i = 0; i < count; i++) bitmaps.Add(image);

    Library filters = new Library(bitmaps);

    SaveBitmaps(filters.TPBrightness(50), "Brightness");
    SaveBitmaps(filters.TContrast(4), "Contrast");
    SaveBitmaps(filters.TGrayscale(), "Grayscale");
    SaveBitmaps(filters.TInvert(), "Invert");
    SaveBitmaps(filters.TSepia(), "Sepia");

    SaveBitmaps(filters.TRotate(90), "Rotate90_");
    SaveBitmaps(filters.TRotate(180), "Rotate180_");
    SaveBitmaps(filters.TRotate(270), "Rotate270_");
    SaveBitmaps(filters.TFlip("horizontal"), "horizontal");
    SaveBitmaps(filters.TFlip("vertical"), "vertical");
    #endregion

    Console.WriteLine("Saved");
    Console.ReadLine();
    DeleteFiles($"{savePath}");
    Console.WriteLine("Deleted");
    Console.ReadLine();
    Console.Clear();
}

void DeleteFiles (string Path)
{
    string [] files = Directory.GetFiles(Path);
    foreach (string file in files)
        File.Delete(file);
}

void SaveBitmaps(List<Bitmap> bitmaps, string name)
{
    int index = 1;
    foreach (var b in bitmaps)
    {
        b.Save($"{savePath}{name}{index}.bmp");
        index++;
    }
}

public class Library
{
    private Bitmap bitmap;
    private object Locker = new object();
    private List<Bitmap> bmp { get; set; }

    public Library(string path)
    {
        if (path.Contains(".png"))
        {
           bitmap = (Bitmap)Image.FromFile(path);
           Bitmap hp = new Bitmap(bitmap.Width, bitmap.Height);
            for (int i = 0; i< bitmap.Width; i++)
            {
                for (int j = 0; j<bitmap.Height; j++)
                {
                    hp.SetPixel(i, j, bitmap.GetPixel(i, j));
                }
            }
            bitmap = (Bitmap)hp.Clone();
        }
        else
            bitmap = new Bitmap(path);
    }

    public Library (List <Bitmap> bitmaps)
    {
        bmp = bitmaps;
    }

    #region Sequentional
    public Bitmap Brightness (int brghtnss)
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
    public Bitmap Contrast (int contrst)
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
    public Bitmap Grayscale ()
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
    public Bitmap Invert ()
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
    public Bitmap Rotate (int degree)
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
    public Bitmap Flip(string rotate)
    {
        Bitmap bmap1;

        lock (Locker)
        {
            bmap1 = (Bitmap)bitmap.Clone();

            if (rotate == "horizontal") bmap1.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (rotate == "vertical") bmap1.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }
        return bmap1;
    }
    #endregion

    #region Parallel.Foreach
    public List<Bitmap> PBrightness(int brghtnss)
    {
        List<Bitmap> result = new List<Bitmap>();
        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Brightness(brghtnss));
        });
        return result;
    }
    public List<Bitmap> PContrast(int contrst)
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Contrast(contrst));
        });
        return result;
    }
    public List<Bitmap> PGrayscale()
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Grayscale());
        });
        return result;
    }
    public List<Bitmap> PInvert()
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Invert());
        });
        return result;
    }
    public List<Bitmap> PSepia()
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Sepia());
        });
        return result;
    }
    public List<Bitmap> PRotate(int degree)
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Rotate(degree));
        });
        return result;
    }
    public List<Bitmap> PFlip(string rotate)
    {
        List<Bitmap> result = new List<Bitmap>();

        Parallel.ForEach(bmp, currentElement => {
            bitmap = currentElement;
            result.Add(Flip(rotate));
        });
        return result;
    }
    #endregion

    #region Thread
    public List<Bitmap> TBrightness (int brghtnss)
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => { result.Add(Brightness(brghtnss)); }));

        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TContrast(int contrst)
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Contrast(contrst))));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TGrayscale()
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Grayscale())));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TInvert()
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Invert())));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TSepia()
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Sepia())));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TRotate(int degree)
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Rotate(degree))));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    public List<Bitmap> TFlip(string rotate)
    {
        List<Bitmap> result = new List<Bitmap>();
        List<Thread> threads = new List<Thread>();

        foreach (var v in bmp)
        {
            bitmap = v;
            threads.Add(new Thread(() => result.Add(Flip(rotate))));
        }
        foreach (var t in threads)
            t.Start();
        foreach (var t in threads)
            t.Join();

        return result;
    }
    #endregion

    #region Pool
    public List<Bitmap> TPBrightness(int brghtnss)
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Brightness(brghtnss));
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPContrast(int contrst)
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Contrast(contrst));
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPGrayscale()
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Grayscale());
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPInvert()
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Invert());
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPSepia()
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Sepia());
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPRotate(int degree)
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Rotate(degree));
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    public List<Bitmap> TPFlip(string rotate)
    {
        List<Bitmap> result = new List<Bitmap>();
        using (CountdownEvent cde = new CountdownEvent(1))
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            foreach (var v in bmp)
            {
                bool enqueued = ThreadPool.QueueUserWorkItem(delegate
                {
                    cde.AddCount();
                    bitmap = v;
                    result.Add(Flip(rotate));
                    cde.Signal();
                    manualResetEvent.Set();
                });
            }
            manualResetEvent.WaitOne();
            cde.Signal();
            cde.Wait();
        }
        return result;
    }
    #endregion
}
