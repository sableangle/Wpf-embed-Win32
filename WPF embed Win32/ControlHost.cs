using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace WPF_embed_Win32
{
    class ControlHost : HwndHost
    {
        IntPtr hwndControl;
        IntPtr hwndHost;
        int hostHeight, hostWidth;
        public Process _process { get; set; }


        const int GWL_STYLE = (-16);

        const UInt32 WS_POPUP = 0x80000000;
        const UInt32 WS_CHILD = 0x40000000;
        const int WS_CAPTION = 0x00C00000;
        const UInt32 WS_THICKFRAME = 0x40000;
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);
        public ControlHost(double height, double width)
        {
            hostHeight = (int)height;
            hostWidth = (int)width;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            ProcessStartInfo psi = new ProcessStartInfo("calc.exe");
            psi.WindowStyle = ProcessWindowStyle.Minimized;
            _process = Process.Start(psi);
            _process.WaitForInputIdle();

            // The main window handle may be unavailable for a while, just wait for it
            while (_process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Yield();
            }

            IntPtr notepadHandle = _process.MainWindowHandle;

            int style = GetWindowLong(notepadHandle, GWL_STYLE);
            style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
            style |= ((int)WS_CHILD); // Must be a child window to be hosted

            SetWindowLong(notepadHandle, GWL_STYLE, style);
            SetParent(notepadHandle, hwndParent.Handle);

            this.InvalidateVisual();

            HandleRef hwnd = new HandleRef(this, notepadHandle);
            return hwnd;
        }
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            return IntPtr.Zero;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }
        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);
    }

}
