using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_embed_Win32
{
    /// <summary>
    /// Type2.xaml 的互動邏輯
    /// </summary>
    public partial class Type2 : Window
    {

        //
        int selectedItem;
        IntPtr hwndListBox;
        ControlHost listControl;
        Application app;
        Window myWindow;
        int itemCount;

        public Type2()
        {
            InitializeComponent();
        }

        //Host Patcher


        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int textLength;

            handled = false;
            if (msg == WM_COMMAND)
            {
                switch ((uint)wParam.ToInt32() >> 16 & 0xFFFF) //extract the HIWORD
                {
                    case LBN_SELCHANGE: 
                        StringBuilder itemText = new StringBuilder();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }

        internal const int
          LBN_SELCHANGE = 0x00000001,
          WM_COMMAND = 0x00000111,
          LB_GETCURSEL = 0x00000188,
          LB_GETTEXTLEN = 0x0000018A,
          LB_ADDSTRING = 0x00000180,
          LB_GETTEXT = 0x00000189,
          LB_DELETESTRING = 0x00000182,
          LB_GETCOUNT = 0x0000018B;

        private void EmbedClick(object sender, RoutedEventArgs e)
        {
            app = System.Windows.Application.Current;
            myWindow = app.MainWindow;
            listControl = new ControlHost(ControlHostElement.ActualHeight, ControlHostElement.ActualWidth);
            ControlHostElement.Child = listControl;
            listControl.MessageHook += new HwndSourceHook(ControlMsgFilter);
            itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);

        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
                                               int msg,
                                               IntPtr wParam,
                                               IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
                                               int msg,
                                               int wParam,
                                               [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hwnd,
                                                  int msg,
                                                  IntPtr wParam,
                                                  String lParam);



    }
}
