using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsStalker
{
    public class ScreenCapture
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

            //TODO hash this
            bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
        }
    }
}