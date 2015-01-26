using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ff14bot;

namespace Cardinal
{
    public static class HotkeySender
    {
        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x0101;
        private static IntPtr windowHandle;

        [DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void SendHotkey(Keys hotkey)
        {
            windowHandle = Core.Memory.Process.MainWindowHandle;

            PostMessage(windowHandle, WM_KEYDOWN, (IntPtr)(hotkey), IntPtr.Zero);
            PostMessage(windowHandle, WM_KEYUP, (IntPtr)(hotkey), IntPtr.Zero);
        }
    }
}