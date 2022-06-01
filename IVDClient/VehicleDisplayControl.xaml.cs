using IVSDClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace IVSDClient
{
    /// <summary>
    /// Interaction logic for VehicleDisplayControl.xaml
    /// </summary>
    public partial class VehicleDisplayControl : UserControl
    {
        public int MonitoredVehicleID = -1;
        public IVSDServerCurrentState cs;
        public Vehicle monitoredVehicle = new Vehicle();
        public Line monitoredLine = new Line();
        public List<Station> upcomingStations = new List<Station>();

        public VehicleDisplayControl()
        {
            InitializeComponent();

            //ProcessUpdatedJSON(@"C:\Games\Transport Fever 2\IVSDCurrentState_0.json");
            //UpdateUI();
        }

        public VehicleDisplayControl(int vehicleID) : this()
        {
            MonitoredVehicleID = vehicleID;
        }

        public void ProcessUpdatedCurrentState()
        {
            try
            {
                monitoredVehicle = cs.vehicles.FirstOrDefault(v => v.id == MonitoredVehicleID);
                monitoredLine = cs.lines.FirstOrDefault(l => l.id == monitoredVehicle.line);

                upcomingStations = new List<Station>();
                foreach (int stopID in monitoredLine.stops) //Using a foreach to account for the order of the stops
                {
                    var station = cs.stations.First(s => s.stationGroup == stopID || s.id == stopID);
                    upcomingStations.Add(station);
                }
                
                this.Dispatcher.BeginInvoke(() => { UpdateUI(); });     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void UpdateUI()
        {
            LineNameLabel.Content = monitoredLine.name;

            NextStopLabel.Content = upcomingStations[monitoredVehicle.stopIndex].name;
            NextStopConnectionsLabel.Content = GenerateConnectionText(monitoredVehicle.stopIndex);
            var bc = new BrushConverter();
            if (monitoredVehicle.state == "AT_TERMINAL")
            {
                NextStopLabel.Content = "This is " + NextStopLabel.Content;
                NextStopGrid.Background = Brushes.LightGreen;
                //NextStopLabel.Foreground = Brushes.White;
            }
            else
            {
                NextStopLabel.Content = "Next up " + NextStopLabel.Content;
                NextStopGrid.Background = Brushes.LightGray;
                //NextStopLabel.Foreground = Brushes.Black;
            }
            //NextStopConnectionsLabel.Foreground = NextStopLabel.Foreground;

            int endStopIndex = monitoredVehicle.stopIndex <= upcomingStations.Count / 2 - 1 ? upcomingStations.Count / 2 - 1 : upcomingStations.Count - 1; //Split each line into having two ends for more realism
            EndStationControl.SetUIText("Last stop at " + upcomingStations[endStopIndex].name, GenerateConnectionText(endStopIndex));

            NextStationsStackPanel.Children.Clear();
            NextStationsDimplesStackPanel.Children.Clear();
            for (int i = monitoredVehicle.stopIndex; i <= endStopIndex && NextStationsStackPanel.Children.Count < 12; i++)
            {
                var NextStationTiltedTextControl = new NextStationTiltedTextControl(upcomingStations[i].name);
                NextStationsStackPanel.Children.Add(NextStationTiltedTextControl);
                var nextStationDimplesControl = new NextStationDimplesControl();
                NextStationsDimplesStackPanel.Children.Add(nextStationDimplesControl);
            }

            /*
            NextStationsStackPanel.Children.Clear();
            for (int i = monitoredVehicle.stopIndex + 1; i <= endStopIndex && NextStationsStackPanel.Children.Count < 4; i++)
            {
                var nextStationControl = new NextStationControl(upcomingStations[i].name, GenerateConnectionText(i));
                NextStationsStackPanel.Children.Add(nextStationControl);
            }
            */
        }

        private double Calculate3DDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
        }

        public string GenerateConnectionText(int index)
        {
            string text = "Change for "
                + String.Join(", ",
                    cs.lines.Where(l => l.id != monitoredLine.id
                        && (l.stops.Contains(upcomingStations[index].id)
                        || l.stops.Contains(upcomingStations[index].stationGroup)))
                    .Select(l => l.name)
                    .OrderBy(l => l));
            if (text == "Change for ")
                text = "No connections are available";
            return text;
        }
    }
}