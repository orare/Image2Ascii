using Image2Ascii_Gui.Image2AsciiLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image2Ascii_Gui
{
    public partial class Image2AsciiForm : Form
    {
        public Image2AsciiForm()
        {
            InitializeComponent();
        }

        //Open image and feed it to the ascii converter
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image inputImage = Image.FromStream(openFileDialog1.OpenFile());
                ImageAscii.OriginalImage = inputImage;
                if (!backgroundWorkerAsciiConverter.IsBusy)
                {
                    backgroundWorkerAsciiConverter.RunWorkerAsync();
                }
            }
        }

        //updates the ascii image scale with scrollbar
        private void imageScaleBar_Scroll(object sender, EventArgs e)
        {
            if (ImageAscii.OriginalImage != null)
            {
                if(!backgroundWorkerAsciiConverter.IsBusy)
                {
                    backgroundWorkerAsciiConverter.RunWorkerAsync();
                }
            }

        }

        //called by async to build our ascii image
        private StringBuilder buildAsciiImage(BackgroundWorker worker, DoWorkEventArgs e)
        {
            StringBuilder asciiText = new StringBuilder();
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                //scale the image
                double scaledSize = imageScaleBarAsync.Value;

                int newWidth = (int)((double)ImageAscii.OriginalImage.Width * (double)(scaledSize / 100));
                int newHeight = (int)((double)ImageAscii.OriginalImage.Height * (double)(scaledSize / 100));

                Bitmap resizedImage = ImageResizer.ResizeImage(ImageAscii.OriginalImage, newWidth, newHeight);

                Bitmap grayScaledImage = GrayscaleConverter.GrayScaleImage(resizedImage);
                asciiText = AsciiConverter.ImageToAscii(grayScaledImage);

            }
            return asciiText;
        }

        //populates textfield with our 'ascii image'
        private void backgroundWorkerAsciiConverter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                asciiTextBox.Text = "Canceled";
            }
            else
            {
                asciiTextBox.Text = e.Result.ToString();
            }
        }

        //call the buildAsciiImage func async
        private void backgroundWorkerAsciiConverter_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = buildAsciiImage(worker, e);
        }

        //copy function
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (asciiTextBox.Text != "")
                Clipboard.SetDataObject(asciiTextBox.Text);
        }

        //save function
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName.ToString());
                file.WriteLine(asciiTextBox.Text);
                file.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
