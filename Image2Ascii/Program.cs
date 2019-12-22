using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Image2Ascii
{
    class Program
    {
        static void Main(string[] args)
        {
            #region File Checks
            if(args.Length == 0)
            {
                Console.WriteLine("<Pleae enter a filename argument.>\n");

                Console.WriteLine("A filename passed alone will keep the image resoultion the same.");
                Console.WriteLine("\te.g. [ image.png ] will keep the original size.\n");

                Console.WriteLine("A single paramter passed alongside the name will scale the Image by a percentage.");
                Console.WriteLine("\te.g. [ image.png 50 ] will scale the image to 50% of its original size.\n");

                Console.WriteLine("Two paramters passed alongside the name will scale the Image to that size.");
                Console.WriteLine("\te.g. [ image.png 60 30 ] will scale the image to a 60 x 30 size.\n");

                Console.WriteLine("<Pleae enter a filename argument.>");
                return;
            }
            string filename = args[0];
            if (!File.Exists(filename))
            {
                Console.WriteLine($"File with the name {filename} could not be found.");
                return;
            }
            #endregion

            //get image width and height for resizing if they are provided



            #region Image Creation
            //open file as an Image object and transform it into a Bitmap.
            //Bitmap objects allow us to edit the image pixels.
            Bitmap grayImage;
            using (Image inputImage = Image.FromFile(filename))
            {
                int newWidth = inputImage.Width;
                int newgHeight = inputImage.Height;
                double scaledSize = 0;

                bool sizeChanged = false;

                //A Single size parameter scales the Image by that percentage.
                if (args.Length == 2 && double.TryParse(args[1], out scaledSize) && scaledSize < 100 && scaledSize > 0)
                {
                    newWidth = (int)((double)inputImage.Width * (double)(scaledSize / 100));
                    newgHeight = (int)((double)inputImage.Height * (double)(scaledSize / 100));
                    sizeChanged = true;
                }
                // Two size parameters scales the image to that resoulution
                if (args.Length == 3 && int.TryParse(args[1], out newWidth )
                                    && int.TryParse(args[2], out newgHeight)
                                    && newWidth > 0 && newgHeight > 0)
                {
                    sizeChanged = true;
                }

                //create a new bitmap ojbect using either the ResizeImage function or just a new instance.
                Bitmap bitmap;
                if (sizeChanged)
                {
                    bitmap = ResizeImage(inputImage, newWidth, newgHeight);
                }
                else
                {
                    bitmap = new Bitmap(inputImage);
                }
                //We grayscale the image so we can sample brightness levels of pixels more reliably.
                grayImage = GrayScaleImage(bitmap);
            }
            #endregion


            //converts the image into an ascii representation and outputs the 'image' to the console and a new file.
            StringBuilder asciiText = ImageToAscii(grayImage);
            Console.WriteLine(asciiText.ToString());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter($"{filename}.txt"))
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

            // Map a brightness value for each pixel to a character ramp for grayscale.
            //http://paulbourke.net/dataformats/asciiart/
            string grayScaleMap = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,"" ^`'. ";
            for (int y = 0; y < grayImage.Height; y++)
            {
                for (int x = 0; x < grayImage.Width; x++)
                {
                    Color pixelC = grayImage.GetPixel(x, y);
                    int brightness = (int) (pixelC.GetBrightness() * (grayScaleMap.Length-1));
                    char letter = grayScaleMap[brightness];
                    asciiText.Append(letter);

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

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        /// https://stackoverflow.com/a/24199315
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
