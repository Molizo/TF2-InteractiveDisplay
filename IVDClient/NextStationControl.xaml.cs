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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IVSDClient
{
    /// <summary>
    /// Interaction logic for NextStationControl.xaml
    /// </summary>
    public partial class NextStationControl : UserControl
    {
        public NextStationControl()
        {
            InitializeComponent();
        }

        public NextStationControl(string stationText, string connectionsText)
        {
            InitializeComponent();
            NextStopLabel.Content = stationText;
            NextStopConnectionsLabel.Content = connectionsText;
        }

        public void SetUIText(string stationText, string connectionsText)
        {
            NextStopLabel.Content = stationText;
            NextStopConnectionsLabel.Content = connectionsText;
        }
    }
}