﻿using IVSDClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Ookii.Dialogs.Wpf;

namespace IVSDClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IVSDServerCurrentState cs;

        private FileSystemWatcher IVSDCurrentStateJSONWatcher;
        private string lastJSONFilePath = "";

        public MainWindow()
        {
            InitializeComponent();

            IVSDCurrentStateJSONWatcher = new FileSystemWatcher();
            IVSDCurrentStateJSONWatcher.NotifyFilter = NotifyFilters.LastWrite;
            IVSDCurrentStateJSONWatcher.Filter = "*.json";
            IVSDCurrentStateJSONWatcher.IncludeSubdirectories = true;
            IVSDCurrentStateJSONWatcher.Changed += new FileSystemEventHandler(OnIVSDCurrentStateJSONChanged);

            //ProcessUpdatedJSON(@"C:\Games\Transport Fever 2\IVSDCurrentState_0.json");
            //UpdateUI();
        }

        private void OnIVSDCurrentStateJSONChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Changed: {e.FullPath}");
            if (lastJSONFilePath != "")
            {
                ProcessUpdatedJSON(lastJSONFilePath);
                this.Dispatcher.BeginInvoke(() => { UpdateUI(); });
            }
            lastJSONFilePath = e.FullPath; //We're doing this to prevent file access conflicts, cycling between multiple JSON files generated by the Transport Fever 2 IVSD mod
        }

        private void ProcessUpdatedJSON(string path)
        {
            using (var fileStream = new FileStream(lastJSONFilePath, FileMode.Open, FileAccess.Read))
                cs = JsonSerializer.Deserialize<IVSDServerCurrentState>(fileStream);
            IVSDCurrentStateJSONWatcher.EnableRaisingEvents = false; //We're making it so it only imports the data once to improve performance, while still avoiding file access conflicts
        }

        private void UpdateUI()
        {
            VehiclesDataGrid.ItemsSource = GenerateVehiclesDataGridContent();
            StationsDataGrid.ItemsSource = GenerateStationsDataGridContent();
        }

        private List<HomePageDataGridVehicle> GenerateVehiclesDataGridContent()
        {
            var vehicles = new List<HomePageDataGridVehicle>();

            foreach (var vehicle in cs.vehicles.Where(v=>v.line!=-1)) //Making sure it's not in a depot
            {
                var v = new HomePageDataGridVehicle()
                {
                    isSelected = false,
                    id = vehicle.id,
                    name = vehicle.name,
                    lineID = vehicle.line,
                    lineName = cs.lines.FirstOrDefault(l => l.id == vehicle.line).name
                };
                vehicles.Add(v);
            }

            return vehicles.OrderBy(c => c.name.Length).ThenBy(c => c.name).ToList();
        }

        private List<HomePageDataGridStation> GenerateStationsDataGridContent()
        {
            var stations = new List<HomePageDataGridStation>();

            foreach (var station in cs.stations)
            {
                var s = new HomePageDataGridStation()
                {
                    isSelected = false,
                    id = station.id,
                    name = station.name,
                    lineNames = String.Join(", ",cs.lines
                        .Where(l => l.stops.Contains(station.id) || 
                                    l.stops.Contains(station.stationGroup))
                        .Select(l => l.name)
                        .OrderBy(c => c.Length)
                        .ThenBy(c => c)
                    )
                };
                stations.Add(s);
            }

            return stations.OrderBy(c => c.name).ToList();
        }

        private void OpenWindowsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedVehicles = VehiclesDataGrid.Items
                .Cast<HomePageDataGridVehicle>()
                .Where(v => v.isSelected)
                .Select(v=>v.id)
                .ToList();
            var selectedStations  = StationsDataGrid.Items
                .Cast<HomePageDataGridStation>()
                .Where(s => s.isSelected)
                .Select(s=>s.id)
                .ToList();

            var dw = new DisplayWindow(IVSDCurrentStateJSONWatcher.Path,selectedVehicles, selectedStations);
            dw.Show();
        }

        // This is so the columns get their name from the model attributes
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }

        private void SelectGameFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select the game folder";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            if ((bool)dialog.ShowDialog(this))
            {
                IVSDCurrentStateJSONWatcher.Path = dialog.SelectedPath;
                IVSDCurrentStateJSONWatcher.EnableRaisingEvents = true; //Only now we allow the file watcher to start watching
            }
        }
    }
}