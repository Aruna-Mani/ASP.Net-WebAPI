using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JayrideAPI.Models
{
    public class ListingsModel
    {
        public class Listing
        {
            public string name { get; set; }
            public double pricePerPassenger { get; set; }
            public VehicleType vehicleType { get; set; }
        }

        public class Root
        {
            public string from { get; set; }
            public string to { get; set; }
            public List<Listing> listings { get; set; }
        }

        public class VehicleType
        {
            public string name { get; set; }
            public int maxPassengers { get; set; }
        }
    }
}