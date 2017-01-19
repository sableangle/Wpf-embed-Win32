using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_embed_Win32
{
    /// <summary>
    /// Type1.xaml 的互動邏輯
    /// </summary>
    public partial class Type1 : Window
    {
        private Process _process;

        public Type1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_THICKFRAME = 0x00040000;
        const UInt32 WS_CHILD = 0x40000000;

        private void Embed_Click(object sender, RoutedEventArgs e)
        {
            //button1.Visibility = Visibility.Hidden;
            
            ProcessStartInfo psi = new ProcessStartInfo("calc.exe");
            _process = Process.Start(psi);
            //_process.WaitForInputIdle();
            while (_process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Yield();
            }

            HwndSource lstHwnd = HwndSource.FromVisual(Canvas) as HwndSource;
            WindowInteropHelper windowHwnd = new WindowInteropHelper(this);
            SetParent(_process.MainWindowHandle, windowHwnd.Handle);
            int style = GetWindowLong(_process.MainWindowHandle, GWL_STYLE);
            style = style & ~((int)WS_CAPTION) & ~((int)WS_THICKFRAME); // Removes Caption bar and the sizing border
            style |= ((int)WS_CHILD); // Must be a child window to be hosted

            SetWindowLong(_process.MainWindowHandle, GWL_STYLE, style);

            // resize embedded application & refresh
            ResizeEmbeddedApp();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_process != null)
            {
                _process.Refresh();
                _process.Close();
            }
        }

        private void ResizeEmbeddedApp()
        {
            if (_process == null)
                return;

            SetWindowPos(_process.MainWindowHandle, IntPtr.Zero, 0, 0, (int)Canvas.Width, (int)Canvas.Height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = base.MeasureOverride(availableSize);
            ResizeEmbeddedApp();
            return size;
        }
    }
}
