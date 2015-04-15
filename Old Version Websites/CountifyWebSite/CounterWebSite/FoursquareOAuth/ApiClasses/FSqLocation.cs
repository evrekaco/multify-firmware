using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    public class FSqLocation
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }
}
