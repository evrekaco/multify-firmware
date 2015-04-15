using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    public class FSqVenue
    {
        public string id { get; set; }
        public string name { get; set; }
        public FSqContact contact { get; set; }
        public FSqLocation location { get; set; }
        //public List<Category> categories { get; set; }
        public bool verified { get; set; }
        public FSqStats stats { get; set; }
        public string url { get; set; }
    }
}
