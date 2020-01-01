using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2Ascii_Gui.Image2AsciiLogic
{
    public static class ImageAscii
    {
        public static Image OriginalImage { get; set; }
        public static Bitmap grayscaleImage { get; set; }
        public static StringBuilder asciiText { get; set; }
    }
}
