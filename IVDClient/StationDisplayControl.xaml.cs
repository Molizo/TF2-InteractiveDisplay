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
    /// Interaction logic for StationDisplayControl.xaml
    /// </summary>
    public partial class StationDisplayControl : UserControl
    {
        public int MonitoredStationID = -1;
        public IVSDServerCurrentState cs;
        public Station monitoredStation = new Station();
        public List<Vehicle> nextArrivingVehicles = new List<Vehicle>();


        public StationDisplayControl()
        {
            InitializeComponent();

            //ProcessUpdatedJSON(@"C:\Games\Transport Fever 2\IVSDCurrentState_0.json");
            //UpdateUI();
        }

        public StationDisplayControl(int stationID) : this()
        {
            MonitoredStationID = stationID;         
        }

        public void ProcessUpdatedCurrentState()
        {
            try
            {
                monitoredStation = cs.stations.FirstOrDefault(v => v.id == MonitoredStationID);

                nextArrivingVehicles = new List<Vehicle>();
                foreach (var vehicle in cs.vehicles.Where(v => v.line != -1)) //Using custom foreach to not calculate the line twice as is the case if we use LINQ
                {
                    var line = cs.lines.FirstOrDefault(l => l.id == vehicle.line);
                    if (line.stops[vehicle.stopIndex] == monitoredStation.id ||
                        line.stops[vehicle.stopIndex] == monitoredStation.stationGroup)
                        nextArrivingVehicles.Add(vehicle);
                }

                nextArrivingVehicles = nextArrivingVehicles
                    .OrderBy(v => Calculate3DDistance(v.position[0], v.position[1], v.position[2],
                        monitoredStation.position[0], monitoredStation.position[1], monitoredStation.position[2]))
                    .ThenBy(v =>
                        v.state !=
                        "AT_TERMINAL") //This is so that trains that are in the station are at the top of the list
                    .ToList();
                
                this.Dispatcher.BeginInvoke(() => { UpdateUI(); });    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        private void UpdateUI()
        {
            StationNameLabel.Content = monitoredStation.name;
            
            NextArrivalsWrapPanel.Children.Clear();
            foreach(var vehicle in nextArrivingVehicles){
                var vehicleLine = cs.lines.FirstOrDefault(l => l.id == vehicle.line);
                string timeLeft = "";
                if (vehicle.state == "AT_TERMINAL")
                    timeLeft = "Here";
                else
                    timeLeft = Math.Round(Calculate3DDistance(vehicle.position[0], vehicle.position[1], vehicle.position[2],
                        monitoredStation.position[0], monitoredStation.position[1], monitoredStation.position[2])/1000,0)+"m";
                int towardsStopID = vehicleLine.stops[(vehicle.stopIndex + 1) % vehicleLine.stops.Count];
                var NextArrivalControl = new NextArrivalControl(vehicleLine.name, "towards "+cs.stations.FirstOrDefault(s=>s.id == towardsStopID || s.stationGroup == towardsStopID).name, timeLeft);

                if (vehicle.state == "AT_TERMINAL")
                    NextArrivalControl.SetColor(true);
                NextArrivalControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                NextArrivalsWrapPanel.Children.Add(NextArrivalControl);
            }
        }
        
        private double Calculate3DDistance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
        }
        
    }
}