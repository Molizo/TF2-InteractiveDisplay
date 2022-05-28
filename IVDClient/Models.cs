﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace IVDClient.Models
{
    public class IVDServerCurrentState
    {
        public List<Vehicle> vehicles { get; set; }
        public List<Station> stations { get; set; }
        public List<Line> lines { get; set; }
    }

    public class Line
    {
        [DefaultValue(10)]
        public int id { get; set; }

        [DefaultValue("")]
        public string name { get; set; }

        [DefaultValue(new int[0])]
        public List<int> stops { get; set; }
    }

    public class Station
    {
        [DefaultValue(20)]
        public int id { get; set; }

        [DefaultValue("")]
        public string name { get; set; }

        [DefaultValue(21)]
        public int stationGroup { get; set; }

        [DefaultValue(new double[0])]
        public List<double> position { get; set; }
    }

    public class Vehicle
    {
        [DefaultValue(30)]
        public int id { get; set; }

        [DefaultValue(-1)]
        public int depot { get; set; }

        [DefaultValue(10)]
        public int line { get; set; }

        [DefaultValue("")]
        public string name { get; set; }

        [DefaultValue("AT_TERMINAL")]
        public string state { get; set; }

        [DefaultValue(0)]
        public int stopIndex { get; set; }

        [DefaultValue(new double[0])]
        public List<double> position { get; set; }
    }

    public class HomePageDataGridVehicle
    {
        [DisplayName("Vehicle ID")]
        [Description("Vehicle ID")]
        [DefaultValue(30)]
        internal int id { get; set; }

        [DisplayName("Vehicle name")]
        [Description("Vehicle name")]
        [DefaultValue("")]
        public string name { get; set; }

        [DisplayName("Line ID")]
        [Description("Line ID")]
        [DefaultValue(30)]
        internal int lineID { get; set; }

        [DisplayName("Line name")]
        [Description("Line name")]
        [DefaultValue("")]
        public string lineName { get; set; }
    }
}