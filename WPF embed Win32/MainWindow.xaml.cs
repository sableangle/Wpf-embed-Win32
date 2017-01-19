using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_embed_Win32
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Type01_click(object sender, RoutedEventArgs e)
        {
            Type1 t = new Type1();
            t.ShowDialog();
        }

        private void Type02_click(object sender, RoutedEventArgs e)
        {
            Type2 t = new Type2();
            t.ShowDialog();
        }
    }
}
