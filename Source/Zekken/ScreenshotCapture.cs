using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ff14bot;
using TreeSharp;

namespace Cardinal
{
    public class ScreenshotCapture
    {
        private const int WAIT_BEFORE_SCREENSHOT = 300;
        private const int SW_RESTORE = 9;
        private IntPtr windowHandle;

        public ScreenshotCapture()
        {
            FileTools.PrepareDirectory(Directories.SCREENSHOT_DIR);
            windowHandle = Core.Memory.Process.MainWindowHandle;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public RunStatus CaptureScreenshot(AvoidanceContext context)
        {
            if (context.UnknownCast == null) { return RunStatus.Success; }
            string name = string.Format("{0} ({1})", context.UnknownCast.SpellId, context.UnknownCast.Name);
            TakeScreenshot(name);

            return RunStatus.Success;
        }

        public void TakeScreenshot(string name)
        {
            var image = CaptureApplication();

            if (image == null)
            {
                Logger.ZekkenMessage("Could not capture screenshot.");
                return;
            }

            string path = Path.Combine(Directories.SCREENSHOT_DIR, name + ".png");
            if (!FileTools.PrepareSave(path)) { return; }

            try
            {
                using (var file = new FileStream(path, FileMode.Create))
                {
                    image.Save(file, ImageFormat.Png);
                }
            }
            catch
            {
                Logger.ZekkenMessage("Could not save screenshot file.");
                return;
            }

            Logger.ZekkenMessage("Saved screenshot of unknown spell {0}.", name);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private Bitmap CaptureApplication()
        {
            HotkeySender.SendHotkey(Keys.Scroll);

            SetForegroundWindow(windowHandle);
            ShowWindow(windowHandle, SW_RESTORE);
            Thread.Sleep(400);

            var rect = new Rect();
            var error = GetWindowRect(windowHandle, ref rect);

            uint processId;
            GetWindowThreadProcessId(windowHandle, out processId);
            var process = Process.GetProcessById((int)processId);

            while (error == (IntPtr)0)
            {
                if (process.HasExited) { return null; }
                error = GetWindowRect(windowHandle, ref rect);
            }

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb555);
            Graphics.FromImage(bmp).CopyFromScreen(rect.left, rect.top, 0, 0,
                new Size(width, height), CopyPixelOperation.SourceCopy);

            HotkeySender.SendHotkey(Keys.Scroll);

            return bmp;
        }

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
