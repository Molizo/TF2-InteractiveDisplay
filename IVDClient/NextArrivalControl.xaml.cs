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
    /// Interaction logic for NextArrivalControl.xaml
    /// </summary>
    public partial class NextArrivalControl : UserControl
    {
        public NextArrivalControl()
        {
            InitializeComponent();
        }

        public NextArrivalControl(string lineName, string towardsLabel, string timeLeftLabel)
        {
            InitializeComponent();
            LineNameLabel.Content = lineName;
            TowardsLabel.Content = towardsLabel;
            TimeLeftLabel.Content = timeLeftLabel;
        }

        public void SetUIText(string lineName, string towardsLabel, string timeLeftLabel)
        {
            LineNameLabel.Content = lineName;
            TowardsLabel.Content = towardsLabel;
            TimeLeftLabel.Content = timeLeftLabel;
        }

        public void SetColor(bool isInStation)
        {
            if (isInStation)
            {
                MainBorder.Background = Brushes.LightGreen;
                //LineNameLabel.Foreground = TowardsLabel.Foreground = TimeLeftLabel.Foreground = Brushes.White;
            }
        }
    }
}