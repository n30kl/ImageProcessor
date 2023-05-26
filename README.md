# ImageProcessor
The library allows you to process one or more graphic images in .bmp, .jpg, .png formats. The library can process images in the following directions: changing brightness and contrast, applying a grayscale filter and sepia tone, obtaining inverted colors, rotating and flipping the image.

Well... Hi!!!!!
It`s my first library i'd newer develop.
You can use in on all of .Net projects (not shure).
So, here is some tips hiw to use it:

1) Create new project using c#;
2) Upload NuGet package, named [ImageProcessor_by_n30_kl] (mine, to process images) & [System.Drawing.Common] (bi Microsoft, for using images);
3) Add next Usings:
  using ImageProcessor;
  using System.Drawing;
4) Create new Class example: 
  - Processing Filter = new Processing([path]), where path is yuor path to image, for example @"C:\img.png" 
  OR if you need to process several images, use: 
  - . . . new Processing([list]), where list is List <Bitmap>;
5) Use it!
  Filter.Grayscale()
6) BTW, you can save it in next way:  Filter.Grayscale().Save([path]), where path - is path to new existing file, for example - @"C:\filter.png";

//IF you process List, you can save in, using this simple method: 
  void SaveBitmaps(List<Bitmap> bitmaps, string name)
{
    int index = 1;
    foreach (var b in bitmaps)
    {
        b.Save($"{savePath}{name}{index}.bmp");
        index++;
    }
}
  
THANKS FOR YOUR ATTANTION. I will glad, if this would be useful for you.
  If you have any questions - contact me: valeriadovgan@gmail.com
 
