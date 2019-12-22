using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Image2Ascii
{
    class Program
    {
        static void Main(string[] args)
        {

            Bitmap GrayImage;
            using (Image inputImage = Image.FromFile("Shapes.png"))
            {
                Bitmap bitmap = new Bitmap(inputImage);
                GrayImage = GrayScaleImage(bitmap);
            }
            GrayImage.Save("test.png");
        }

        /// <summary>
        /// Returns a new Bitmap object with grayscaled colors.
        /// </summary>
        /// https://stackoverflow.com/a/4004527
        private static Bitmap GrayScaleImage(Bitmap origBitmap)
        {
            Bitmap grayBitmap = new Bitmap(origBitmap.Width, origBitmap.Height);

            for (int x = 0; x < origBitmap.Width; x++)
            {
                for (int y = 0; y < origBitmap.Height; y++)
                {
                    Color pixelColor = origBitmap.GetPixel(x, y);
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    Color newColor = Color.FromArgb(pixelColor.A, grayScale, grayScale, grayScale);
                    grayBitmap.SetPixel(x, y, newColor);
                }
            }

            return grayBitmap;
        }

    }
}
