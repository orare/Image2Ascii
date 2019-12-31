using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2Ascii_Gui.Image2AsciiLogic
{
    public static class AsciiConverter
    {
        public static StringBuilder ImageToAscii(Bitmap grayImage)
        {
            StringBuilder asciiText = new StringBuilder();

            // Map a brightness value for each pixel to a character ramp for grayscale.
            //http://paulbourke.net/dataformats/asciiart/
            string grayScaleMap = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,"" ^`'. ";
            for (int y = 0; y < grayImage.Height; y++)
            {
                for (int x = 0; x < grayImage.Width; x++)
                {
                    Color pixelC = grayImage.GetPixel(x, y);
                    int brightness = (int)(pixelC.GetBrightness() * (grayScaleMap.Length - 1));
                    char letter = grayScaleMap[brightness];
                    asciiText.Append(letter);

                }
                asciiText.Append("\n");
            }
            return asciiText;
        }
    }
}
