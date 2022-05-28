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

namespace IVDClient
{
    /// <summary>
    /// Interaction logic for NextStationTiltedTextControl.xaml
    /// </summary>
    public partial class NextStationTiltedTextControl : UserControl
    {
        public NextStationTiltedTextControl()
        {
            InitializeComponent();
        }

        public NextStationTiltedTextControl(string stationText) : this()
        {
            if (stationText.Length > 17)
                stationText = stationText.Substring(0, 17);
            NextStopTextBlock.Text = stationText;
        }

        public void SetUIText(string stationText)
        {
            if (stationText.Length > 17)
                stationText = stationText.Substring(0, 17);
            NextStopTextBlock.Text = stationText;
        }
    }
}