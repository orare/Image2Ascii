using Image2Ascii_Gui.Image2AsciiLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image2Ascii_Gui
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(
  ControlStyles.AllPaintingInWmPaint |
  ControlStyles.UserPaint |
  ControlStyles.DoubleBuffer, true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filepath = openFileDialog1.FileName;
                Image inputImage = Image.FromStream(openFileDialog1.OpenFile());
                ImageAscii.OriginalImage = inputImage;
            }
        }

        private void imageScaleBar_Scroll(object sender, EventArgs e)
        {
            if(ImageAscii.OriginalImage != null)
            {
                StringBuilder asciiText;
                double scaledSize = imageScaleBar.Value;

                int newWidth = (int)((double)ImageAscii.OriginalImage.Width * (double)(scaledSize / 100));
                int newHeight = (int)((double)ImageAscii.OriginalImage.Height * (double)(scaledSize / 100));

                Bitmap resizedImage = ImageResizer.ResizeImage(ImageAscii.OriginalImage, newWidth, newHeight);

                Bitmap grayScaledImage = GrayscaleConverter.GrayScaleImage(resizedImage);
                asciiText = AsciiConverter.ImageToAscii(grayScaledImage);

                asciiTextBox.Text = asciiText.ToString();
                
            }

        }

        private void asciiTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
