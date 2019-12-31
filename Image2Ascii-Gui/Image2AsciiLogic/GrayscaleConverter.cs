using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2Ascii_Gui.Image2AsciiLogic
{
    public static class GrayscaleConverter
    {
        public static Bitmap GrayScaleImage(Bitmap origBitmap)
        {
            Bitmap grayBitmap = new Bitmap(origBitmap.Width, origBitmap.Height);

            //loop through every pixel in the image so we can apply a grayscale 'filter' to it.
            for (int x = 0; x < origBitmap.Width; x++)
            {
                for (int y = 0; y < origBitmap.Height; y++)
                {
                    Color pixelColor = origBitmap.GetPixel(x, y);
                    //you can look at https://en.wikipedia.org/wiki/Grayscale if you want to know where these values came from.
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    Color newColor = Color.FromArgb(pixelColor.A, grayScale, grayScale, grayScale);
                    grayBitmap.SetPixel(x, y, newColor);
                }
            }

            return grayBitmap;
        }

    }
}
