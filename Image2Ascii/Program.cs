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

            Bitmap grayImage;
            using (Image inputImage = Image.FromFile("Shapes.png"))
            {
                Bitmap bitmap = new Bitmap(inputImage);
                grayImage = GrayScaleImage(bitmap);
            }
            grayImage.Save("test.png");

            StringBuilder asciiText = ImageToAscii(grayImage);
            Console.WriteLine(asciiText.ToString());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("ascii.txt"))
            {
                file.WriteLine(asciiText.ToString()); 
            }


        }

        /// <summary>
        /// Takes a Bitmap Image and returns a StringBuilder that respresents the image in ASCII characters.
        /// </summary>
        /// <param name="grayImage"></param>
        /// <returns></returns>
        private static StringBuilder ImageToAscii(Bitmap grayImage)
        {
            StringBuilder asciiText = new StringBuilder();

            for (int y = 0; y < grayImage.Height; y++)
            {
                for (int x = 0; x < grayImage.Width; x++)
                {
                    Color pixelC = grayImage.GetPixel(x, y);
                    if (pixelC.GetBrightness() > 0.1)
                    {
                        asciiText.Append("#");
                    }
                    else
                    {
                        asciiText.Append(".");
                    }
                }
                asciiText.Append("\n");
            }

            return asciiText;
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
