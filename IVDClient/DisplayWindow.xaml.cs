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

namespace IVSDClient
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        
        private bool isMaximized = false;
        
        public DisplayWindow()
        {
            InitializeComponent(); 
        }

        public DisplayWindow(string watchFolder, List<int> vehicles, List<int> stations) : this()
        {
            MainUniformGrid.Children.Clear();
            foreach (var i in vehicles)
            {
                var vdc = new VehicleDisplayControl(watchFolder,i);
                MainUniformGrid.Children.Add(vdc);
            }
            foreach (var i in stations)
            {
                var sdc = new StationDisplayControl(watchFolder,i);
                MainUniformGrid.Children.Add(sdc);
            }
        }

        private void Window_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isMaximized)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
            isMaximized = !isMaximized;
        }
    }
}
